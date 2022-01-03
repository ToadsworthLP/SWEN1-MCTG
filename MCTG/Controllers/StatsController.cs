using MCTG.Auth;
using MCTG.Config;
using MCTG.Responses;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/stats")]
    internal class StatsController
    {
        private readonly AppDbContext db;

        public StatsController(AppDbContext db)
        {
            this.db = db;
        }

        [Method(Method.GET)]
        [Restrict(Role.USER)]
        public IApiResponse ShowOwnStats()
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            return new Ok(new UserStatsResponse(AuthProvider.CurrentUser.BattleCount, AuthProvider.CurrentUser.Elo));
        }
    }
}
