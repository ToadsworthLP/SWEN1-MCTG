using Data.SQL;
using MCTG.Config;
using MCTG.Models;

namespace MCTG.Services
{
    public class DeckService : IDeckService
    {
        private AppDbContext db;

        public DeckService(AppDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<DeckEntry> GetUserDeckEntries(User user)
        {
            return new SelectCommand<DeckEntry>().From(db.DeckEntries).WhereEquals(nameof(DeckEntry.Owner), user.Id).Run(db);
        }

        public IEnumerable<Card> GetUserDeck(User user)
        {
            return GetUserDeckEntries(user).Select((entry) => db.Cards.Get(entry.Card));
        }
    }
}
