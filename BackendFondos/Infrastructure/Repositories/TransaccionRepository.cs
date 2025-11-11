using Amazon.DynamoDBv2.DataModel;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Infrastructure.Dynamo;

namespace BackendFondos.Infrastructure.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly DynamoDBContext _context;

        public TransaccionRepository(DynamoDbContext db)
        {
            _context = db.Context;
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorClienteAsync(string clienteId)
        {
            var query = _context.QueryAsync<Transaccion>(clienteId, new DynamoDBOperationConfig
            {
                IndexName = "GSI_ClienteID"
            });

            return await query.GetRemainingAsync();

        }

        public async Task<Transaccion?> ObtenerPorIdAsync(string clienteId, string transaccionId)
        {
            return await _context.LoadAsync<Transaccion>(clienteId, transaccionId);
        }

        public async Task CrearAsync(Transaccion transaccion)
        {
            await _context.SaveAsync(transaccion);
        }

        public async Task EliminarAsync(string clienteId, string transaccionId)
        {
            await _context.DeleteAsync<Transaccion>(clienteId, transaccionId);
        }
    }
}