namespace MCTG.Gameplay
{
    public class BattleSummary
    {
        public BattleResult Result { get; set; }
        public List<string> Log { get; set; }

        public BattleSummary(BattleResult result, List<string> log)
        {
            Result = result;
            Log = log;
        }

        public enum BattleResult { P1WON, P2WON, DRAW }
    }
}
