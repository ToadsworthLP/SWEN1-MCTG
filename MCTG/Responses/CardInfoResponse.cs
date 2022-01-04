using MCTG.Models;

namespace MCTG.Responses
{
    public class CardInfoResponse
    {
        public Guid Id;
        public string Name;
        public string Type;
        public double Damage;

        public CardInfoResponse(Card card)
        {
            Id = card.Id;
            Name = card.Name == null ? "" : card.Name;
            Type = card.Type;
            Damage = card.Damage;
        }

        public CardInfoResponse(Card card, string name)
        {
            Id = card.Id;
            Name = name;
            Type = card.Type;
            Damage = card.Damage;
        }

        public CardInfoResponse(Guid id, string name, string type, double damage)
        {
            Id = id;
            Name = name;
            Type = type;
            Damage = damage;
        }
    }
}
