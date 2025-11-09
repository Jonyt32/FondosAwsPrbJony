using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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

        public async Task<IEnumerable<Cliente?>> ObtenerTodosAsync() 
        {
            var scan = _context.ScanAsync<Cliente>(new List<ScanCondition>());
            return await scan.GetRemainingAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(string clienteId)
        {
            return await _context.LoadAsync<Cliente>(clienteId);
        }

        public async Task<Cliente?> ObtenerClientePorEmailAsync(string email)
        {
            try
            {
                var request = new QueryRequest
                {
                    TableName = "Clientes",
                    IndexName = "GSI_Email",
                    KeyConditionExpression = "Email = :v_email",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":v_email", new AttributeValue { S = email } }
                    }
                };

                var client = new AmazonDynamoDBClient();
                var response = await client.QueryAsync(request);

                var item = response.Items.FirstOrDefault();
                if (item == null) return null;

                var usuario = new Cliente
                {
                    ClienteID = item["ClienteID"].S,
                    Email = item["Email"].S,
                    Nombre = item["Nombre"].S,
                    Saldo = decimal.TryParse(item["Saldo"].N, out var saldo) ? saldo : 0m,
                    PreferenciaNotificacion = item.ContainsKey("PreferenciaNotificacion") ? item["PreferenciaNotificacion"].S : "email",
                    FondosActivos = item.ContainsKey("FondosActivos") && item["FondosActivos"].SS != null
                        ? new HashSet<string>(item["FondosActivos"].SS)
                        : new HashSet<string>(),
                            CanalesNotificacion = item.ContainsKey("CanalesNotificacion") && item["CanalesNotificacion"].M != null
                        ? item["CanalesNotificacion"].M.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.S)
                        : new Dictionary<string, string>()

                };

                return usuario;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
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