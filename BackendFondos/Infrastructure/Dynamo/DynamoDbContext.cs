using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace BackendFondos.Infrastructure.Dynamo
{
    public class DynamoDbContext
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public DynamoDbContext()
        {
            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1 // Cambia según tu región
            };

            _client = new AmazonDynamoDBClient(config);
            _context = new DynamoDBContext(_client);
        }

        public DynamoDBContext Context => _context;
    }
}