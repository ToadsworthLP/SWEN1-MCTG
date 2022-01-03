using MCTG.Gameplay;
using MCTG.Models;

namespace MCTG.Services
{
    internal class BattleService : IBattleService
    {
        private readonly CardTypeRegistry cardTypeRegistry;
        private readonly ICardNameService cardNameService;

        public BattleService(CardTypeRegistry cardTypeRegistry, ICardNameService cardNameService)
        {
            this.cardTypeRegistry = cardTypeRegistry;
            this.cardNameService = cardNameService;
        }

        public BattleSummary Battle(string name1, IEnumerable<Card> deck1, string name2, IEnumerable<Card> deck2)
        {
            return new BattleSummary(BattleSummary.BattleResult.DRAW, new List<string>());
        }
    }
}
