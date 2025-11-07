using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Infrastructure.Dynamo;
using BCrypt.Net;
using FastEndpoints;


namespace BackendFondos.Infrastructure.Repositories
{
    public class UsuarioRepository: IUsuarioRepository
    {
        private readonly DynamoDBContext _context;
        private const string EmailGsiName = "GSI_Email";
        public UsuarioRepository(DynamoDbContext db)
        {
            _context = db.Context;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            var scan = _context.ScanAsync<Usuario>(new List<ScanCondition>());
            return await scan.GetRemainingAsync();
        }

        public async Task<Usuario> ObtenerPorIdAsync(string userId)
        {
            return await _context.LoadAsync<Usuario>(userId);
        }

        public async Task<Usuario?> LoginAsync(string email, string password)
        {
            try
            {
                var usuario = await ObtenerUsuarioPorEmail(email);
                var isValid = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
                return isValid ? usuario : null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<Usuario> ObtenerUsuarioPorEmail(string email) 
        {
            try
            {
                var request = new QueryRequest
                {
                    TableName = "Usuarios",
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

                var usuario = new Usuario
                {
                    UsuarioID = item["UsuarioID"].S,
                    Email = item["Email"].S,
                    PasswordHash = item["Password"].S,
                    NombreUsuario = item["NombreUsuario"].S,
                    Rol = item["Rol"].S
                };

                return usuario;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            var user = new Usuario
            {
                UsuarioID = Guid.NewGuid().ToString(),
                Email = usuario.Email,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol ?? "User",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash, workFactor: 12) 
            };

            await _context.SaveAsync(user).ConfigureAwait(false);
            user.PasswordHash = string.Empty;
            return user;
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            await _context.SaveAsync(usuario);
        }

        public async Task EliminarAsync(string userId)
        {
            await _context.DeleteAsync<Cliente>(userId);
        }
    }
}
