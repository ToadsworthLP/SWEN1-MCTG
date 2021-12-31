using Data;
using MCTG.Auth;

namespace MCTG.Models
{
    internal record User : DbRecord
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public int Elo { get; set; }
        public Role Role { get; set; }

        public User() { }

        public User(string username, byte[] passwordHash, Role role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            Elo = 100;
        }

        public User(string username, byte[] passwordHash, Role role, string? bio, string? image, int elo)
        {
            Username = username;
            PasswordHash = passwordHash;
            Bio = bio;
            Image = image;
            Elo = elo;
        }
    }
}
