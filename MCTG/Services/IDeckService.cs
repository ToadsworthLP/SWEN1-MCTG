using MCTG.Models;

namespace MCTG.Services
{
    internal interface IDeckService
    {
        IEnumerable<Card> GetUserDeck(User user);
        IEnumerable<DeckEntry> GetUserDeckEntries(User user);
    }
}