namespace MCTG.Gameplay.CardTypes
{
    public class Kraken : ICardType
    {
        public CardCategory Category => CardCategory.MONSTER;

        public CardElement Element => CardElement.WATER;

        public double GetDefendDamageMultiplier(ICardType opponent)
        {
            if (opponent.Category == CardCategory.SPELL) return 0.0;

            return 1.0;
        }
    }
}
