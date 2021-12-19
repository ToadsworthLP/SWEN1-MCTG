using Data;
using MCTG.Models;

namespace MCTG.Config
{
    internal class AppDbContext : DbContext
    {
        public const string ConnectionString = "Server=localhost;User Id=mctg;Password=pwd;Database=mctg;";

        public DbSet<TestModel> TestModels;

        public AppDbContext() : base(ConnectionString)
        {
            Bind("test", out TestModels);
        }
    }
}
