using Data;
using MCTG.Auth;
using MCTG.Gameplay;
using MCTG.Models;

namespace MCTG.Config
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users;
        public DbSet<Card> Cards;
        public DbSet<PackageEntry> PackageEntries;
        public DbSet<Package> Packages;
        public DbSet<DeckEntry> DeckEntries;
        public DbSet<TradeOffer> TradeOffers;
        public DbSet<LobbyEntry> Lobby;

        static AppDbContext()
        {
            MapEnum<Role>("role");
            MapEnum<CardCategory>("card_category");
            MapEnum<CardElement>("card_element");
        }

        public AppDbContext()
        {
            Bind("user", out Users);
            Bind("card", out Cards);
            Bind("package", out Packages);
            Bind("package_entry", out PackageEntries);
            Bind("deck_entry", out DeckEntries);
            Bind("trade_offer", out TradeOffers);
            Bind("lobby", out Lobby);
        }
    }
}
