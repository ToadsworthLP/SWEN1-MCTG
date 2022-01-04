using MCTG.Gameplay.CardTypes;

namespace MCTG.Gameplay
{
    public class CardTypeRegistry
    {
        private readonly Dictionary<string, ICardType> cardTypes;

        public CardTypeRegistry()
        {
            cardTypes = new Dictionary<string, ICardType>();
        }

        public void Register(string id, ICardType type)
        {
            cardTypes.Add(id, type);
        }

        public ICardType? Get(string id)
        {
            if (cardTypes.ContainsKey(id)) return cardTypes[id];
            return null;
        }
    }
}
