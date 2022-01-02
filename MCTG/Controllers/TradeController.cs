using Data.SQL;
using MCTG.Auth;
using MCTG.Config;
using MCTG.Gameplay;
using MCTG.Models;
using MCTG.Requests;
using MCTG.Responses;
using MCTG.Services;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/trading")]
    internal class TradeController
    {
        private readonly AppDbContext db;
        private readonly ICardNameService cardNameService;
        private readonly CardTypeRegistry cardTypeRegistry;
        private readonly ICardUsageCheckService cardUsageCheckService;

        public TradeController(AppDbContext db, ICardNameService cardUsageCheckService, CardTypeRegistry cardTypeRegistry, ICardUsageCheckService cardLockCheckService)
        {
            this.db = db;
            this.cardNameService = cardUsageCheckService;
            this.cardTypeRegistry = cardTypeRegistry;
            this.cardUsageCheckService = cardLockCheckService;
        }

        [Method(Method.GET)]
        [Restrict(Role.USER)]
        public IApiResponse ShowOffers()
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            IEnumerable<TradeOffer> tradeOffers = new SelectCommand<TradeOffer>().From(db.TradeOffers).Run(db);

            IList<TradeOfferInfoResponse> responses = new List<TradeOfferInfoResponse>();
            foreach (TradeOffer offer in tradeOffers)
            {
                Card card = db.Cards.Get(offer.Offered);
                User partner = db.Users.Get((Guid)card.Owner);

                responses.Add(new TradeOfferInfoResponse(partner.Id, new CardInfoResponse(card, cardNameService.GetName(card)), offer.Category, offer.MinDamage));
            }

            return new Ok(responses);
        }

        [Method(Method.POST)]
        [Restrict(Role.USER)]
        public IApiResponse AddOffer([FromBody] AddTradeOfferRequest request)
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            Card? card = db.Cards.Get(request.CardToTrade);

            if (card == null || card.Owner != AuthProvider.CurrentUser.Id || cardUsageCheckService.IsInDeck(card) || cardUsageCheckService.IsInTradeOffer(card))
            {
                return new NotFound(new ErrorResponse($"The card {request.CardToTrade} does not exist or is currently in use."));
            }

            if (request.Id == null)
            {
                db.TradeOffers.Create(new TradeOffer(card.Id, request.Category, request.MinDamage));
            }
            else
            {
                db.TradeOffers.Create(new TradeOffer((Guid)request.Id, card.Id, request.Category, request.MinDamage));
            }

            try
            {
                db.Commit();
                return new Ok();
            }
            catch (Exception ex)
            {
                return new BadRequest(new ErrorResponse($"An error occured: {ex}"));
            }
        }

        [Method(Method.DELETE)]
        [Restrict(Role.USER)]
        public IApiResponse DeleteOffer([FromRoute] string offerIdStr)
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            Guid offerId;
            try
            {
                offerId = Guid.Parse(offerIdStr);
            }
            catch (Exception)
            {
                return new BadRequest(new ErrorResponse("Invalid trade offer ID."));
            }

            TradeOffer? offer = db.TradeOffers.Get(offerId);

            if (offer == null) return new NotFound(new ErrorResponse($"No trade offer with ID {offerId} found."));

            if (db.Cards.Get(offer.Offered)?.Owner != AuthProvider.CurrentUser.Id) return new Unauthorized("Cannot delete someone else's trade offer.");

            db.TradeOffers.Delete(offer);
            db.Commit();

            return new Ok();
        }
    }
}
