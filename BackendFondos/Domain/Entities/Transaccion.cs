using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;



namespace BackendFondos.Domain.Entities
{
    [DynamoDBTable("Transacciones")]
    public class Transaccion
    {
        [DynamoDBHashKey]
        public string TransaccionID { get; set; }
        [DynamoDBProperty]
        public string ClienteID { get; set; } = string.Empty;
        [DynamoDBProperty(Converter = typeof(EnumAsStringConverter<TipoTransaccion>))]

        public TipoTransaccion Tipo { get; set; }
        [DynamoDBProperty]
        public string FondoID { get; set; } = string.Empty;
        [DynamoDBProperty]
        public decimal Monto { get; set; }
        [DynamoDBProperty]
        public DateTime Fecha { get; set; }
        [DynamoDBProperty]
        public string Notificacion { get; set; } = "email";
    }

    public class EnumAsStringConverter<T> : IPropertyConverter where T : struct, Enum
    {
        public DynamoDBEntry ToEntry(object value)
        {
            return new Primitive(value.ToString());
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            return Enum.Parse(typeof(T), entry.AsString());
        }
    }

}
