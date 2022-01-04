namespace MCTG.Responses
{
    public class ScoreboardResponse
    {
        public IEnumerable<ScoreboardEntry> Entries;

        public ScoreboardResponse(IEnumerable<ScoreboardEntry> entries)
        {
            Entries = entries;
        }

        public record ScoreboardEntry(int Place, string Name, int Elo);
    }
}
