namespace MCTG.Responses
{
    internal class CardInfoResponse
    {
        public Guid Id;
        public string Name;
        public string Type;
        public double Damage;

        public CardInfoResponse(Guid id, string name, string type, double damage)
        {
            Id = id;
            Name = name;
            Type = type;
            Damage = damage;
        }
    }
}
