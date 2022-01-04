using Data;

namespace MCTG.Models
{
    public record PackageEntry : DbRecord
    {
        public Guid Package { get; set; }
        public Guid Card { get; set; }

        public PackageEntry() { }

        public PackageEntry(Guid card)
        {
            Card = card;
        }

        public PackageEntry(Guid package, Guid card)
        {
            Package = package;
            Card = card;
        }
    }
}
