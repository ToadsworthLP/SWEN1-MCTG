using Data;
using MCTG.Gameplay;

namespace MCTG.Models
{
    public record TradeOffer : DbRecord
    {
        public Guid Offered { get; set; }
        public CardCategory Category { get; set; }
        public double MinDamage { get; set; }

        public TradeOffer() { }

        public TradeOffer(Guid tradeId, Guid cardToTrade, CardCategory category, double minDamage)
        {
            Id = tradeId;
            Offered = cardToTrade;
            Category = category;
            MinDamage = minDamage;
        }

        public TradeOffer(Guid cardToTrade, CardCategory category, double minDamage)
        {
            Offered = cardToTrade;
            Category = category;
            MinDamage = minDamage;
        }
    }
}
