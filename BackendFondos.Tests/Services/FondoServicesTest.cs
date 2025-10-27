using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Domain.Services;

namespace BackendFondos.Tests.Services
{
    public class FondoServicesTest
    {
        [Fact]
        public async Task CrearFondo_DeberiaCrearCorrectamente()
        {
            var fondo = new Fondo { FondoID = "f1", NombreFondo = "Fondo A", MontoMinimo = 100 };

            var fondoRepoMock = new Mock<IFondoRepository>();
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("f1")).ReturnsAsync((Fondo)null);
            fondoRepoMock.Setup(r => r.CrearAsync(fondo)).Returns(Task.CompletedTask);

            var service = new FondoService(fondoRepoMock.Object);

            await service.CrearFondoAsync(fondo);

            fondoRepoMock.Verify(r => r.CrearAsync(fondo), Times.Once);
        }

        [Fact]
        public async Task CrearFondo_DeberiaLanzarExcepcion_SiFondoYaExiste()
        {
            var fondo = new Fondo { FondoID = "f1", NombreFondo = "Fondo A", MontoMinimo = 100 };

            var fondoRepoMock = new Mock<IFondoRepository>();
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("f1")).ReturnsAsync(fondo);

            var service = new FondoService(fondoRepoMock.Object);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CrearFondoAsync(fondo));
            Assert.Equal("El fondo ya existe", ex.Message);
        }

        [Fact]
        public async Task CrearFondo_DeberiaLanzarExcepcion_SiMontoMinimoEsInvalido()
        {
            var fondo = new Fondo { FondoID = "f2", NombreFondo = "Fondo B", MontoMinimo = 0 };

            var fondoRepoMock = new Mock<IFondoRepository>();
            var service = new FondoService(fondoRepoMock.Object);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CrearFondoAsync(fondo));
            Assert.Equal("El monto mínimo debe ser mayor a cero", ex.Message);
        }

        [Fact]
        public async Task ObtenerFondo_DeberiaRetornarFondoCorrecto()
        {
            var fondo = new Fondo { FondoID = "f1", NombreFondo = "Fondo A", MontoMinimo = 100 };

            var fondoRepoMock = new Mock<IFondoRepository>();
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("f1")).ReturnsAsync(fondo);

            var service = new FondoService(fondoRepoMock.Object);

            var resultado = await service.ObtenerFondoPorIdAsync("f1");

            Assert.NotNull(resultado);
            Assert.Equal("Fondo A", resultado.NombreFondo);
        }

        [Fact]
        public async Task ObtenerFondo_DeberiaLanzarExcepcion_SiNoExiste()
        {
            var fondoRepoMock = new Mock<IFondoRepository>();
            fondoRepoMock.Setup(r => r.ObtenerPorIdAsync("fX")).ReturnsAsync((Fondo)null);

            var service = new FondoService(fondoRepoMock.Object);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.ObtenerFondoPorIdAsync("fX"));
            Assert.Equal("Fondo no encontrado", ex.Message);
        }
    }
}