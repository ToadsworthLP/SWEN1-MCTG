using Data;

namespace MCTG.Models
{
    internal record Card : DbRecord
    {
        public string? Name { get; set; }
        public double Damage { get; set; }
        public string Type { get; set; }
        public Guid? Owner { get; set; }

        public Card() { }

        public Card(double damage, string type)
        {
            Damage = damage;
            Type = type;
        }

        public Card(string? name, double damage, string type)
        {
            Name = name;
            Damage = damage;
            Type = type;
        }

        public Card(Guid id, string? name, double damage, string type)
        {
            Id = id;
            Name = name;
            Damage = damage;
            Type = type;
        }
    }
}
