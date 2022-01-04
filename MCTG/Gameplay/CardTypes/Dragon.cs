namespace MCTG.Gameplay.CardTypes
{
    public class Dragon : ICardType
    {
        public CardCategory Category => CardCategory.MONSTER;

        public CardElement Element => CardElement.FIRE;
    }
}
