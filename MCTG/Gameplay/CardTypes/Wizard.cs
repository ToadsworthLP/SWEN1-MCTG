namespace MCTG.Gameplay.CardTypes
{
    public class Wizard : ICardType
    {
        public CardCategory Category => CardCategory.MONSTER;

        public CardElement Element { get; set; }

        public double GetDefendDamageMultiplier(ICardType opponent)
        {
            if (opponent is Ork) return 0.0;

            return 1.0;
        }

        public Wizard(CardElement element)
        {
            Element = element;
        }
    }
}
