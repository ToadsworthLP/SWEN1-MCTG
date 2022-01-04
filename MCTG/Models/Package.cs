using Data;

namespace MCTG.Models
{
    public record Package : DbRecord
    {
        public int Price { get; set; }

        public Package() { }

        public Package(int price)
        {
            Price = price;
        }
    }
}
