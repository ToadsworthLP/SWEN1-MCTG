namespace MCTG.Responses
{
    internal class ScoreboardResponse
    {
        public IEnumerable<ScoreboardEntry> Entries;

        public ScoreboardResponse(IEnumerable<ScoreboardEntry> entries)
        {
            Entries = entries;
        }

        internal record ScoreboardEntry(int Place, string Name, int Elo);
    }
}
