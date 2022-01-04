namespace MCTG.Gameplay.CardTypes
{
    public class Elf : ICardType
    {
        public CardCategory Category => CardCategory.MONSTER;

        public CardElement Element { get; set; }

        public double GetDefendDamageMultiplier(ICardType opponent)
        {
            if (Element == CardElement.FIRE && opponent is Dragon) return 0.0;

            return 1.0;
        }

        public Elf(CardElement element)
        {
            Element = element;
        }
    }
}
