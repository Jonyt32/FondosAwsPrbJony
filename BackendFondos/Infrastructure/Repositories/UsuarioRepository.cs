using Amazon.DynamoDBv2.DataModel;
using BackendFondos.Domain.Entities;
using BackendFondos.Domain.Repositories;
using BackendFondos.Infrastructure.Dynamo;
using BCrypt.Net;


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

        public async Task<Usuario> ObtenerPorIdAsync(string userId)
        {
            return await _context.LoadAsync<Usuario>(userId);
        }

        public async Task<Usuario?> LoginAsync(string email, string password)
        {
            try
            {
                var cfg = new DynamoDBOperationConfig { IndexName = EmailGsiName };
                var q = _context.QueryAsync<Usuario>(email, cfg);
                var list = await q.GetNextSetAsync().ConfigureAwait(false);
                var user = list.FirstOrDefault();
                if (user == null) return null;

                if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    return user;

                return null;
            }
            catch (Amazon.DynamoDBv2.AmazonDynamoDBException)
            {
                var all = await _context.ScanAsync<Usuario>(new List<ScanCondition>()).GetRemainingAsync().ConfigureAwait(false);
                var user = all.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
                if (user == null) return null;
                if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    return user;
                return null;
            }

        }

        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            var user = new Usuario
            {
                UsuarioID = Guid.NewGuid().ToString(),
                Email = usuario.Email,
                UserName = usuario.UserName,
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
