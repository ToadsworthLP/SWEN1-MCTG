using MCTG.Auth;
using MCTG.Config;
using MCTG.Models;
using MCTG.Responses;
using MCTG.Services;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;
using System.Text;

namespace MCTG.Controllers
{
    [Route("/deck")]
    internal class DeckController
    {
        private readonly AppDbContext db;
        private readonly ICardNameService cardNameService;
        private readonly ICardUsageCheckService cardUsageCheckService;
        private readonly IDeckService deckService;

        public DeckController(AppDbContext db, ICardNameService cardNameService, ICardUsageCheckService cardUsageCheckService, IDeckService deckService)
        {
            this.db = db;
            this.cardNameService = cardNameService;
            this.cardUsageCheckService = cardUsageCheckService;
            this.deckService = deckService;
        }

        [Method(Method.GET)]
        [Restrict(Role.USER)]
        public IApiResponse ShowCurrentDeck([FromParameter("format")] string? format)
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            IEnumerable<Card> deck = deckService.GetUserDeck(AuthProvider.CurrentUser);

            if (format == "plain")
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (Card card in deck)
                {
                    stringBuilder.Append($"{cardNameService.GetName(card)} (Damage: {card.Damage})\n");
                }

                return new Ok(stringBuilder.ToString());
            }
            else
            {
                return new Ok(deck.Select((card) => new CardInfoResponse(card, cardNameService.GetName(card))));
            }
        }

        [Method(Method.PUT)]
        [Restrict(Role.USER)]
        public IApiResponse SetDeck([FromBody] Guid[] cardIds)
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            if (cardIds.Length != Constants.DECK_SIZE)
            {
                return new BadRequest(new ErrorResponse($"The deck must consist of exactly {Constants.DECK_SIZE} cards."));
            }

            // Validate and get list of cards to put in the deck
            IList<Card> cards = new List<Card>();
            foreach (Guid cardId in cardIds)
            {
                Card? card = db.Cards.Get(cardId);

                if (card == null || card.Owner != AuthProvider.CurrentUser.Id || cardUsageCheckService.IsInTradeOffer(card))
                {
                    return new NotFound(new ErrorResponse($"The card {cardId} does not exist or is currently in use."));
                }

                cards.Add(card);
            }

            // Clear current deck
            IEnumerable<DeckEntry> currentDeck = deckService.GetUserDeckEntries(AuthProvider.CurrentUser);
            foreach (DeckEntry entry in currentDeck)
            {
                db.DeckEntries.Delete(entry);
            }

            // Add new deck entries
            foreach (Card card in cards)
            {
                db.DeckEntries.Create(new DeckEntry(AuthProvider.CurrentUser.Id, card.Id));
            }

            db.Commit();
            return new Ok();
        }
    }
}
