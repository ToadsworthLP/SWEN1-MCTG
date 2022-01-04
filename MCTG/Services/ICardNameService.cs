using MCTG.Models;

namespace MCTG.Services
{
    public interface ICardNameService
    {
        string GetName(Card card);
    }
}