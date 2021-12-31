using Data;
using MCTG.Auth;
using MCTG.Models;

namespace MCTG.Config
{
    internal class AppDbContext : DbContext
    {
        public const string ConnectionString = "Server=localhost;User Id=mctg;Password=pwd;Database=mctg;";

        public DbSet<User> Users;

        static AppDbContext()
        {
            MapEnum<Role>("role");
        }

        public AppDbContext() : base(ConnectionString)
        {
            Bind("user", out Users);
        }
    }
}
