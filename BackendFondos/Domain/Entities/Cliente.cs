using Amazon.DynamoDBv2.DataModel;

namespace BackendFondos.Domain.Entities
{
    [DynamoDBTable("Clientes")]
    public class Cliente
    {
        [DynamoDBHashKey] 
        public string ClienteID { get; set; }

        [DynamoDBProperty]
        public string Nombre { get; set; }

        [DynamoDBProperty]
        public HashSet<string> FondosActivos { get; set; } = new();

        [DynamoDBProperty]
        public string PreferenciaNotificacion { get; set; } = "email";

        [DynamoDBProperty]
        public string Email { get; set; } = string.Empty;

        [DynamoDBProperty]
        public decimal Saldo { get; set; }

        [DynamoDBProperty]
        public Dictionary<string, string> CanalesNotificacion { get; set; } = new();

        public decimal CalcularSaldoDisponible(IEnumerable<Fondo> fondosActivos)
        {
            var totalComprometido = fondosActivos.Sum(f => f.MontoMinimo);
            return Saldo - totalComprometido;
        }
    }
}