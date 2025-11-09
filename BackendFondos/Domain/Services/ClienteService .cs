using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace BackendFondos.Domain.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepo;

        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task CrearClienteAsync(Cliente cliente)
        {
            try
            {
                // Generar un ID único si no viene definido
                cliente.ClienteID ??= Guid.NewGuid().ToString();

                if (string.IsNullOrWhiteSpace(cliente.ClienteID))
                    throw new InvalidOperationException("El ID del cliente es obligatorio");

                if (cliente.Saldo < 0)
                    throw new InvalidOperationException("El saldo no puede ser negativo");

                var existente = await _clienteRepo.ObtenerPorIdAsync(cliente.ClienteID);
                if (existente != null)
                    throw new InvalidOperationException("El cliente ya existe");

                cliente.PreferenciaNotificacion ??= "email";
                cliente.FondosActivos ??= new HashSet<string>();

                await _clienteRepo.CrearAsync(cliente);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public async Task<Cliente> ObtenerClientePorIdAsync(string clienteId)
        {
            var cliente = await _clienteRepo.ObtenerPorIdAsync(clienteId);
            return cliente ?? throw new InvalidOperationException("Cliente no encontrado");
        }

        public async Task<List<Cliente>> FiltrarClientes(string email) 
        {
            var lstClientes = new List<Cliente>();
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    var filtro = await ObtenerClientePorEmailAsync(email);
                    lstClientes.Add(filtro);
                }
                else 
                {
                    lstClientes = await ObtenerTodosClientes();
                }
                return lstClientes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<Cliente?>> ObtenerTodosClientes() 
        {
            var lst = await _clienteRepo.ObtenerTodosAsync();
            if (lst == null)
                throw new InvalidOperationException("No existen fondos");

            return lst.ToList();
        }
        private async Task<Cliente?> ObtenerClientePorEmailAsync(string email) 
        {
            try
            {
                return await _clienteRepo.ObtenerClientePorEmailAsync(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task ActualizarClienteAsync(Cliente cliente)
        {
            var existente = await _clienteRepo.ObtenerPorIdAsync(cliente.ClienteID);
            if (existente == null)
                throw new InvalidOperationException("Cliente no existe");

            await _clienteRepo.ActualizarAsync(cliente);
        }

        public async Task ActualizarSaldoClienteAsync(string clienteId, decimal monto)
        {
            try
            {
                var existente = await _clienteRepo.ObtenerPorIdAsync(clienteId);

                if (existente == null)
                    throw new InvalidOperationException("Cliente no existe");

                existente.Saldo = monto;
                await _clienteRepo.ActualizarAsync(existente);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task EliminarClienteAsync(string clienteId)
        {
            var existente = await _clienteRepo.ObtenerPorIdAsync(clienteId);
            if (existente == null)
                throw new InvalidOperationException("Cliente no existe");

            await _clienteRepo.EliminarAsync(clienteId);
        }
    }
}

