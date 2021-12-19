using Data;

namespace MCTG.Models
{
    public record TestModel : DbRecord
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public TestModel() { }

        public TestModel(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }
}
