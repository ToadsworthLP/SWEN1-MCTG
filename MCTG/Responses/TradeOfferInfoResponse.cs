using MCTG.Gameplay;

namespace MCTG.Responses
{
    internal class TradeOfferInfoResponse
    {
        public Guid Partner;
        public CardInfoResponse Card;
        public CardCategory Category;
        public double MinDamage;

        public TradeOfferInfoResponse(Guid partner, CardInfoResponse card, CardCategory category, double minDamage)
        {
            Partner = partner;
            Card = card;
            Category = category;
            MinDamage = minDamage;
        }
    }
}
