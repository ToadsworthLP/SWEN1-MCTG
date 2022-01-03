using MCTG.Models;

namespace MCTG.Services
{
    internal interface IEloService
    {
        void UpdateElo(User winner, User loser);
    }
}