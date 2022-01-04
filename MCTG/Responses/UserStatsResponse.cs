namespace MCTG.Responses
{
    public class UserStatsResponse
    {
        public int BattleCount;
        public int Elo;

        public UserStatsResponse(int battleCount, int elo)
        {
            BattleCount = battleCount;
            Elo = elo;
        }
    }
}
