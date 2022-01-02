using MCTG.Models;

namespace MCTG.Services
{
    internal interface ICardNameService
    {
        string GetName(Card card);
    }
}