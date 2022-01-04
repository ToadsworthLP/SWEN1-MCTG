namespace MCTG.Gameplay.CardTypes
{
    public class Goblin : ICardType
    {
        public CardCategory Category => CardCategory.MONSTER;

        public CardElement Element { get; set; }

        public double GetAttackDamageMultiplier(ICardType opponent)
        {
            if (opponent is Dragon) return 0.0;

            return 1.0;
        }

        public Goblin(CardElement element)
        {
            Element = element;
        }
    }
}
