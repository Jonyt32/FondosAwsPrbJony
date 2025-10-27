using Amazon.DynamoDBv2.DataModel;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Infrastructure.Dynamo;

namespace BackendFondos.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DynamoDBContext _context;

        public ClienteRepository(DynamoDbContext db)
        {
            _context = db.Context;
        }

        public async Task<Cliente?> ObtenerPorIdAsync(string clienteId)
        {
            return await _context.LoadAsync<Cliente>(clienteId);
        }

        public async Task CrearAsync(Cliente cliente)
        {
            await _context.SaveAsync(cliente);
        }

        public async Task ActualizarAsync(Cliente cliente)
        {
            await _context.SaveAsync(cliente);
        }

        public async Task EliminarAsync(string clienteId)
        {
            await _context.DeleteAsync<Cliente>(clienteId);
        }
    }
}