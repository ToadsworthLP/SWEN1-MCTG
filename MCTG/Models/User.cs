using Data;
using MCTG.Auth;

namespace MCTG.Models
{
    public record User : DbRecord
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public int BattleCount { get; set; }
        public int Elo { get; set; }
        public Role Role { get; set; }
        public int Coins { get; set; }

        public User() { }

        public User(string username, byte[] passwordHash, Role role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            BattleCount = 0;
            Elo = 100;
            Coins = 20;
        }

        public User(string username, byte[] passwordHash, Role role, string? bio, string? image, int battleCount, int elo, int coins)
        {
            Username = username;
            PasswordHash = passwordHash;
            Bio = bio;
            Image = image;
            BattleCount = battleCount;
            Elo = elo;
            Coins = coins;
        }
    }
}
