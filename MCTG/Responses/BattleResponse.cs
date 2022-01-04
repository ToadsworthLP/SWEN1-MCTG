namespace MCTG.Responses
{
    public class BattleResponse
    {
        public Guid? Winner;
        public Guid? Loser;
        public List<string> Log;

        public BattleResponse(Guid? winner, Guid? loser, List<string> log)
        {
            Winner = winner;
            Loser = loser;
            Log = log;
        }
    }
}
