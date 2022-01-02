using MCTG.Models;
using System.Text;

namespace MCTG.Services
{
    internal class CardNameService : ICardNameService
    {
        public string GetName(Card card)
        {
            if (card.Name != null) return card.Name;

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < card.Type.Length; i++)
            {
                char c = card.Type[i];
                if (char.IsUpper(c) && i != 0) builder.Append(' ');
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
