using Data.SQL;
using MCTG.Config;
using MCTG.Models;

namespace MCTG.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext db;

        public TokenService(AppDbContext db)
        {
            this.db = db;
        }

        public string GenerateToken(User user)
        {
            return $"{user.Username}-mtcgToken";
        }

        public User? ReadToken(string token)
        {
            if (!token.EndsWith("-mtcgToken")) return null;

            string username = token.Substring(0, token.Length - 10);
            return new SelectCommand<User>().From(db.Users).WhereEquals(nameof(User.Username), username).Run(db).FirstOrDefault();
        }

        public void InvalidateToken(string token)
        {
            // Would be used for implementing logout - not needed in this project
            throw new NotImplementedException();
        }
    }
}
