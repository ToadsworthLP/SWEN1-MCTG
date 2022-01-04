using MCTG.Gameplay;
using MCTG.Models;

namespace MCTG.Services
{
    public interface IBattleService
    {
        BattleSummary Battle(string name1, IEnumerable<Card> deck1, string name2, IEnumerable<Card> deck2);
    }
}