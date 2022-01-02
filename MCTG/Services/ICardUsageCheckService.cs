using MCTG.Models;

namespace MCTG.Services
{
    internal interface ICardUsageCheckService
    {
        bool IsInDeck(Card card);
        bool IsInTradeOffer(Card card);
    }
}