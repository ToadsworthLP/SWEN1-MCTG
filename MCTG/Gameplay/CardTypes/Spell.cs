namespace MCTG.Gameplay.CardTypes
{
    public class Spell : ICardType
    {
        public CardCategory Category => CardCategory.SPELL;

        public CardElement Element { get; init; }

        public Spell(CardElement element)
        {
            Element = element;
        }
    }
}
