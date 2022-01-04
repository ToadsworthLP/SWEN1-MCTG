using Data.SQL;
using MCTG.Auth;
using MCTG.Config;
using MCTG.Models;
using MCTG.Responses;
using MCTG.Services;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/cards")]
    public class CardsController
    {
        private readonly AppDbContext db;
        private readonly ICardNameService cardNameService;

        public CardsController(AppDbContext db, ICardNameService cardNameService)
        {
            this.db = db;
            this.cardNameService = cardNameService;
        }

        [Method(Method.GET)]
        [Restrict(Role.USER)]
        public IApiResponse ShowOwnCards()
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            IEnumerable<CardInfoResponse> cards = new SelectCommand<Card>().From(db.Cards).WhereEquals(nameof(Card.Owner), AuthProvider.CurrentUser.Id).Run(db)
                .Select((card) => new CardInfoResponse(card.Id, cardNameService.GetName(card), card.Type, card.Damage));

            return new Ok(cards);
        }
    }
}
