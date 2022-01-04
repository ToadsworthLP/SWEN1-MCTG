using MCTG.Config;
using MCTG.Models;

namespace MCTG.Services
{
    public class EloService : IEloService
    {
        private readonly AppDbContext db;

        public EloService(AppDbContext db)
        {
            this.db = db;
        }

        public void UpdateElo(User winner, User loser)
        {
            db.Users.Update(winner with { Elo = winner.Elo + 3 });
            db.Users.Update(loser with { Elo = loser.Elo - 5 });
        }
    }
}
