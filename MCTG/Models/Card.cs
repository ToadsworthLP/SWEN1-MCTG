using Data;

namespace MCTG.Models
{
    internal record Card : DbRecord
    {
        public string? Name { get; set; }
        public int Damage { get; set; }
        public string Type { get; set; }
        public Guid? Owner { get; set; }

        public Card() { }

        public Card(int damage, string type)
        {
            Damage = damage;
            Type = type;
        }

        public Card(Guid id, string? name, int damage, string type)
        {
            Id = id;
            Name = name;
            Damage = damage;
            Type = type;
        }
    }
}
