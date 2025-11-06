using Amazon.DynamoDBv2.DataModel;

namespace BackendFondos.Domain.Entities
{
    [DynamoDBTable("Usuarios")]
    public class Usuario
    {
        [DynamoDBHashKey]
        public string UsuarioID { get; set; }
        [DynamoDBProperty]
        public string UserName { get; set; }

        [DynamoDBProperty]
        public string Email { get; set; } = string.Empty;

        [DynamoDBProperty("Password")]
        public string PasswordHash { get; set; }

        [DynamoDBProperty]
        public string Rol { get; set; } = "User";

        
    }
}
