using Data.SQL;
using MCTG.Auth;
using MCTG.Config;
using MCTG.Models;
using MCTG.Responses;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;
using static MCTG.Responses.ScoreboardResponse;

namespace MCTG.Controllers
{
    [Route("/score")]
    internal class ScoreboardController
    {
        private readonly AppDbContext db;

        public ScoreboardController(AppDbContext db)
        {
            this.db = db;
        }

        [Method(Method.GET)]
        [Restrict(Role.USER)]
        public IApiResponse ShowScoreboard()
        {
            IEnumerable<ScoreboardEntry> entries = new SelectCommand<User>().From(db.Users).OrderByDescending(nameof(User.Elo)).Run(db)
                .Select((user, index) => new ScoreboardEntry(index + 1, user.Username, user.Elo));

            return new Ok(new ScoreboardResponse(entries));
        }
    }
}
