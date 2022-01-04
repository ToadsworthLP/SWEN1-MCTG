namespace MCTG.Responses
{
    public class UserProfileResponse
    {
        public string Username;
        public string? Bio;
        public string? Image;
        public int BattleCount;
        public int Elo;

        public UserProfileResponse(string username, string? bio, string? image, int battleCount, int elo)
        {
            Username = username;
            Bio = bio;
            Image = image;
            BattleCount = battleCount;
            Elo = elo;
        }
    }
}
