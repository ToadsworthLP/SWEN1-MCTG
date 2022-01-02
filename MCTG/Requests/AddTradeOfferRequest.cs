using MCTG.Gameplay;

namespace MCTG.Requests
{
    internal class AddTradeOfferRequest
    {
        public Guid? Id;
        public Guid CardToTrade;
        public CardCategory Category;
        public double MinDamage;
    }
}
