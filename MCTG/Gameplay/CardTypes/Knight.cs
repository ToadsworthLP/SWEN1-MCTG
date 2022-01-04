namespace MCTG.Gameplay.CardTypes
{
    public class Knight : ICardType
    {
        public CardCategory Category => CardCategory.MONSTER;

        public CardElement Element => CardElement.NORMAL;

        public double GetDefendDamageMultiplier(ICardType opponent)
        {
            if (opponent.Category == CardCategory.SPELL && opponent.Element == CardElement.WATER) return double.PositiveInfinity;

            return 1.0;
        }
    }
}
