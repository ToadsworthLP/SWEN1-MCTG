namespace MCTG.Responses
{
    internal class UserProfileResponse
    {
        public string Username;
        public string? Bio;
        public string? Image;
        public int Elo;

        public UserProfileResponse(string username, string? bio, string? image, int elo)
        {
            Username = username;
            Bio = bio;
            Image = image;
            Elo = elo;
        }
    }
}
