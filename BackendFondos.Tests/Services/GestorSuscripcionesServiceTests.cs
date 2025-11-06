using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Domain.Services;
using BackendFondos.Domain.Validators;
using BackendFondos.Application.DTOs;

namespace BackendFondos.Tests.Services
{
    public class GestorSuscripcionesServiceTests
    {
        private GestorSuscripcionesService CrearServicioConMocks(
            Mock<IClienteRepository> clienteRepoMock,
            Mock<IFondoRepository> fondoRepoMock,
            Mock<ITransaccionRepository> transaccionRepoMock = null,
            Mock<INotificacionEmailService> notificacionMock = null,
            Mock<ILogService<GestorSuscripcionesService>> logMock = null)
        {
            return new GestorSuscripcionesService(
                clienteRepoMock.Object,
                fondoRepoMock.Object,
                transaccionRepoMock?.Object ?? new Mock<ITransaccionRepository>().Object,
                new SuscripcionValidator(),
                new CancelacionValidator(),
                new TransaccionValidator(),
                notificacionMock?.Object ?? new Mock<INotificacionEmailService>().Object,
                logMock?.Object ?? new Mock<ILogService<GestorSuscripcionesService>>().Object
            );
        }

        [Fact]
        public async Task SuscribirClienteAFondoAsync_DeberiaRetornarError_SiClienteNoExiste()
        {
            var clienteRepoMock = new Mock<IClienteRepository>();
            var fondoRepoMock = new Mock<IFondoRepository>();
            var logMock = new Mock<ILogService<GestorSuscripcionesService>>();

            clienteRepoMock.Setup(r => r.ObtenerPorIdAsync("clienteX")).ReturnsAsync((Cliente)null);
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("fondoY")).ReturnsAsync(new Fondo { FondoID = "fondoY", MontoMinimo = 500 });

            var service = CrearServicioConMocks(clienteRepoMock, fondoRepoMock, logMock: logMock);

            var resultado = await service.SuscribirClienteAFondoAsync("clienteX", "fondoY");

            Assert.False(resultado.Exito);
            Assert.Contains("cliente no existe", resultado.MensajeNotificacion, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task SuscribirClienteAFondoAsync_DeberiaRetornarError_SiSaldoInsuficiente()
        {
            var cliente = new Cliente { ClienteID = "c1", Saldo = 100, FondosActivos = new HashSet<string>() };
            var fondo = new Fondo { FondoID = "f1", MontoMinimo = 500 };

            var clienteRepoMock = new Mock<IClienteRepository>();
            var fondoRepoMock = new Mock<IFondoRepository>();
            var logMock = new Mock<ILogService<GestorSuscripcionesService>>();

            clienteRepoMock.Setup(r => r.ObtenerPorIdAsync("c1")).ReturnsAsync(cliente);
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("f1")).ReturnsAsync(fondo);
            fondoRepoMock.Setup(r => r.ObtenerFondosPorIdsAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new List<Fondo>());

            var service = CrearServicioConMocks(clienteRepoMock, fondoRepoMock, logMock: logMock);

            var resultado = await service.SuscribirClienteAFondoAsync("c1", "f1");

            Assert.False(resultado.Exito);
            Assert.Contains("Saldo insuficiente", resultado.MensajeNotificacion, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task SuscribirClienteAFondoAsync_DeberiaRetornarExito_SiTodoEsValido()
        {
            var cliente = new Cliente
            {
                ClienteID = "c1",
                Nombre = "Jhony",
                Saldo = 1000,
                PreferenciaNotificacion = "email",
                CanalesNotificacion = new Dictionary<string, string> { { "email", "jhony@email.com" } },
                FondosActivos = new HashSet<string>()
            };

            var fondo = new Fondo { FondoID = "f1", NombreFondo = "Fondo Premium", MontoMinimo = 500 };

            var clienteRepoMock = new Mock<IClienteRepository>();
            var fondoRepoMock = new Mock<IFondoRepository>();
            var transaccionRepoMock = new Mock<ITransaccionRepository>();
            var notificacionMock = new Mock<INotificacionEmailService>();
            var logMock = new Mock<ILogService<GestorSuscripcionesService>>();

            clienteRepoMock.Setup(r => r.ObtenerPorIdAsync("c1")).ReturnsAsync(cliente);
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("f1")).ReturnsAsync(fondo);
            fondoRepoMock.Setup(r => r.ObtenerFondosPorIdsAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync(new List<Fondo>());

            var service = CrearServicioConMocks(clienteRepoMock, fondoRepoMock, transaccionRepoMock, notificacionMock, logMock);

            var resultado = await service.SuscribirClienteAFondoAsync("c1", "f1");

            Assert.True(resultado.Exito);
            Assert.Contains("Suscripción exitosa", resultado.MensajeNotificacion);
            notificacionMock.Verify(n => n.EnviarCorreoAsync("jhony@email.com", It.IsAny<string>(), It.IsAny<string>(), TipoTransaccion.Suscripcion),  Times.Once);
        }

        [Fact]
        public async Task CancelarSuscripcionAsync_DeberiaRetornarError_SiFondoNoExiste()
        {
            var cliente = new Cliente { ClienteID = "c1", FondosActivos = new HashSet<string> { "f1" } };

            var clienteRepoMock = new Mock<IClienteRepository>();
            var fondoRepoMock = new Mock<IFondoRepository>();
            var logMock = new Mock<ILogService<GestorSuscripcionesService>>();

            clienteRepoMock.Setup(r => r.ObtenerPorIdAsync("c1")).ReturnsAsync(cliente);
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("f1")).ReturnsAsync((Fondo)null);

            var service = CrearServicioConMocks(clienteRepoMock, fondoRepoMock, logMock: logMock);

            var resultado = await service.CancelarSuscripcionAsync("c1", "f1");

            Assert.False(resultado.Exito);
            Assert.Contains("fondo no existe", resultado.MensajeNotificacion, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task CancelarSuscripcionAsync_DeberiaRetornarExito_SiTodoEsValido()
        {
            var cliente = new Cliente
            {
                ClienteID = "c1",
                Nombre = "Jhony",
                Saldo = 1000,
                PreferenciaNotificacion = "email",
                CanalesNotificacion = new Dictionary<string, string> { { "email", "jhony@email.com" } },
                FondosActivos = new HashSet<string> { "f1" }
            };

            var fondo = new Fondo { FondoID = "f1", NombreFondo = "Fondo Premium", MontoMinimo = 500 };

            var clienteRepoMock = new Mock<IClienteRepository>();
            var fondoRepoMock = new Mock<IFondoRepository>();
            var transaccionRepoMock = new Mock<ITransaccionRepository>();
            var notificacionMock = new Mock<INotificacionEmailService>();
            var logMock = new Mock<ILogService<GestorSuscripcionesService>>();

            clienteRepoMock.Setup(r => r.ObtenerPorIdAsync("c1")).ReturnsAsync(cliente);
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("f1")).ReturnsAsync(fondo);

            var service = CrearServicioConMocks(clienteRepoMock, fondoRepoMock, transaccionRepoMock, notificacionMock, logMock);

            var resultado = await service.CancelarSuscripcionAsync("c1", "f1");

            Assert.True(resultado.Exito);
            Assert.Contains("Cancelación exitosa", resultado.MensajeNotificacion);
            notificacionMock.Verify(n => n.EnviarCorreoAsync("jhony@email.com", It.IsAny<string>(), It.IsAny<string>(), TipoTransaccion.Cancelacion), Times.Once);
        }
    }
}