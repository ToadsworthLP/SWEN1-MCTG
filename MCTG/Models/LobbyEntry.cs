using Data;

namespace MCTG.Models
{
    public record LobbyEntry : DbRecord
    {
        public Guid Opponent { get; set; }

        public LobbyEntry() { }

        public LobbyEntry(Guid opponent)
        {
            Opponent = opponent;
        }
    }
}
