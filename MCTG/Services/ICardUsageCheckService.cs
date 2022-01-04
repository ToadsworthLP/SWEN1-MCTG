using MCTG.Models;

namespace MCTG.Services
{
    public interface ICardUsageCheckService
    {
        bool IsInDeck(Card card);
        bool IsInTradeOffer(Card card);
    }
}