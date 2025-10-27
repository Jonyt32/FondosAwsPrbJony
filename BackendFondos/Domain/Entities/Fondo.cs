using Amazon.DynamoDBv2.DataModel;

namespace BackendFondos.Domain.Entities
{
    [DynamoDBTable("Fondos")]
    public class Fondo
    {
        [DynamoDBHashKey]
        public string FondoID { get; set; }
        [DynamoDBProperty]
        public string NombreFondo { get; set; } = string.Empty;
        [DynamoDBProperty]
        public decimal MontoMinimo { get; set; }
        [DynamoDBProperty]
        public string Categoria { get; set; } = string.Empty;
    }
}
