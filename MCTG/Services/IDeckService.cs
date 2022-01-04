using MCTG.Models;

namespace MCTG.Services
{
    public interface IDeckService
    {
        IEnumerable<Card> GetUserDeck(User user);
        IEnumerable<DeckEntry> GetUserDeckEntries(User user);
    }
}