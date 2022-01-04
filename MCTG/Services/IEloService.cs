using MCTG.Models;

namespace MCTG.Services
{
    public interface IEloService
    {
        void UpdateElo(User winner, User loser);
    }
}