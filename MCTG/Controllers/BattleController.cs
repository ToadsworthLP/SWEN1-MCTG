using Data.SQL;
using MCTG.Auth;
using MCTG.Config;
using MCTG.Gameplay;
using MCTG.Models;
using MCTG.Responses;
using MCTG.Services;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/battle")]
    public class BattleController
    {
        private readonly AppDbContext db;
        private readonly IBattleService battleService;
        private readonly IDeckService deckService;
        private readonly IEloService eloService;

        public BattleController(AppDbContext db, IBattleService battleService, IDeckService deckService, IEloService eloService)
        {
            this.db = db;
            this.battleService = battleService;
            this.deckService = deckService;
            this.eloService = eloService;
        }

        [Method(Method.POST)]
        [Restrict(Role.USER)]
        public IApiResponse EnterLobby()
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            LobbyEntry? opponentEntry = new SelectCommand<LobbyEntry>().From(db.Lobby).Limit(1).Run(db).FirstOrDefault();

            if (opponentEntry == null) // Add a lobby entry
            {
                db.Lobby.Create(new LobbyEntry(AuthProvider.CurrentUser.Id));
                db.Commit();

                return new Accepted();
            }
            else // Start the battle!
            {
                User? opponent = db.Users.Get(opponentEntry.Opponent);
                db.Lobby.Delete(opponentEntry);

                if (opponent == null)
                {
                    db.Commit();
                    return new InternalServerError(new ErrorResponse("Invalid lobby entry."));
                }

                IEnumerable<Card> ownDeck = deckService.GetUserDeck(AuthProvider.CurrentUser);
                IEnumerable<Card> opponentDeck = deckService.GetUserDeck(opponent);

                BattleSummary battleSummary = battleService.Battle(AuthProvider.CurrentUser.Username, ownDeck, opponent.Username, opponentDeck);

                User? winner = null;
                User? loser = null;
                switch (battleSummary.Result)
                {
                    case BattleSummary.BattleResult.P1WON:
                        winner = AuthProvider.CurrentUser with { BattleCount = AuthProvider.CurrentUser.BattleCount + 1 };
                        loser = opponent with { BattleCount = opponent.BattleCount + 1 };
                        break;
                    case BattleSummary.BattleResult.P2WON:
                        winner = opponent with { BattleCount = opponent.BattleCount + 1 };
                        loser = AuthProvider.CurrentUser with { BattleCount = AuthProvider.CurrentUser.BattleCount + 1 };
                        break;
                }

                if (battleSummary.Result != BattleSummary.BattleResult.DRAW)
                {
                    eloService.UpdateElo(winner, loser);
                }
                else // not exactly a great way to get around missing incremental changes in the data layer, but it works
                {
                    db.Users.Update(AuthProvider.CurrentUser with { BattleCount = AuthProvider.CurrentUser.BattleCount + 1 });
                    db.Users.Update(opponent with { BattleCount = opponent.BattleCount + 1 });
                }

                db.Commit();

                BattleResponse response = new BattleResponse(
                    battleSummary.Result == BattleSummary.BattleResult.DRAW ? null : winner.Id,
                    battleSummary.Result == BattleSummary.BattleResult.DRAW ? null : loser.Id,
                    battleSummary.Log);

                return new Ok(response);
            }
        }
    }
}
