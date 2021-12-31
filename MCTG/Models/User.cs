using Data;

namespace MCTG.Models
{
    internal record User : DbRecord
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? Nickname { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public int Elo { get; set; }

        public User() { }

        public User(string username, byte[] passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
            Elo = 100;
        }

        public User(string username, byte[] passwordHash, string? nickname, string? bio, string? image, int elo)
        {
            Username = username;
            PasswordHash = passwordHash;
            Nickname = nickname;
            Bio = bio;
            Image = image;
            Elo = elo;
        }
    }
}
