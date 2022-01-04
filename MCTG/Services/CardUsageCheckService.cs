using Data.SQL;
using MCTG.Config;
using MCTG.Models;

namespace MCTG.Services
{
    public class CardUsageCheckService : ICardUsageCheckService
    {
        private readonly AppDbContext db;

        public CardUsageCheckService(AppDbContext db)
        {
            this.db = db;
        }

        public bool IsInDeck(Card card)
        {
            return new SelectCommand<DeckEntry>().From(db.DeckEntries).WhereEquals(nameof(DeckEntry.Card), card.Id).Run(db).Any();
        }

        public bool IsInTradeOffer(Card card)
        {
            return new SelectCommand<TradeOffer>().From(db.TradeOffers).WhereEquals(nameof(TradeOffer.Offered), card.Id).Run(db).Any();
        }
    }
}
