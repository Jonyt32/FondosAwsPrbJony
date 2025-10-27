using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Infrastructure.Dynamo;

namespace BackendFondos.Infrastructure.Repositories
{
    public class FondoRepository : IFondoRepository
    {
        private readonly DynamoDBContext _context;
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly string _tablaFondos;


        public FondoRepository(DynamoDbContext db, IAmazonDynamoDB dynamoDb, IConfiguration config)
        {
            _context = db.Context;
            _tablaFondos = config["DynamoDB:TablaFondos"];
            _dynamoDb = dynamoDb;
        }

        public async Task<IEnumerable<Fondo>> ObtenerTodosAsync()
        {
            var scan = _context.ScanAsync<Fondo>(new List<ScanCondition>());
            return await scan.GetRemainingAsync();
        }

        public async Task<Fondo?> ObtenerPorIdAsync(string fondoId)
        {
            return await _context.LoadAsync<Fondo>(fondoId);
        }

        public async Task CrearAsync(Fondo fondo)
        {
            await _context.SaveAsync(fondo);
        }

        public async Task ActualizarAsync(Fondo fondo)
        {
            await _context.SaveAsync(fondo);
        }

        public async Task EliminarAsync(string fondoId)
        {
            await _context.DeleteAsync<Fondo>(fondoId);
        }

        public async Task<IEnumerable<Fondo>> ObtenerFondosPorIdsAsync(IEnumerable<string> ids)
        {
            var keys = ids.Select(id => new Dictionary<string, AttributeValue>
                        {
                            { "FondoId", new AttributeValue { S = id } }
                        }).ToList();

            var request = new BatchGetItemRequest
            {
                RequestItems = new Dictionary<string, KeysAndAttributes>
                {
                    {
                        _tablaFondos,
                        new KeysAndAttributes { Keys = keys }
                    }
                }
            };

            var response = await _dynamoDb.BatchGetItemAsync(request);

            var items = response.Responses[_tablaFondos];

            var fondos = items.Select(item => new Fondo
            {
                FondoID = item["FondoId"].S,
                NombreFondo = item["NombreFondo"].S,
                MontoMinimo = decimal.Parse(item["MontoMinimo"].N),
                Categoria = item["Categoria"].S
            });

            return fondos;

        }

    }
}