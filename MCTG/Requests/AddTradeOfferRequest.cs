using MCTG.Gameplay;

namespace MCTG.Requests
{
    public class AddTradeOfferRequest
    {
        public Guid? Id;
        public Guid CardToTrade;
        public CardCategory Category;
        public double MinDamage;
    }
}
