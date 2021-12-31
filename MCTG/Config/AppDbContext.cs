using Data;
using MCTG.Auth;
using MCTG.Models;

namespace MCTG.Config
{
    internal class AppDbContext : DbContext
    {

        public DbSet<User> Users;

        static AppDbContext()
        {
            MapEnum<Role>("role");
        }

        public AppDbContext()
        {
            Bind("user", out Users);
        }
    }
}
