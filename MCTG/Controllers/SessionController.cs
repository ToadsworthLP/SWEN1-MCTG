using Data.SQL;
using MCTG.Config;
using MCTG.Models;
using MCTG.Requests;
using MCTG.Responses;
using MCTG.Services;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/sessions")]
    public class SessionController
    {
        private readonly AppDbContext db;
        private readonly IPasswordHashService passwordHashService;
        private readonly ITokenService tokenService;

        public SessionController(AppDbContext db, IPasswordHashService passwordHashService, ITokenService tokenService)
        {
            this.db = db;
            this.passwordHashService = passwordHashService;
            this.tokenService = tokenService;
        }

        [Method(Method.POST)]
        public IApiResponse Login([FromBody] LoginUserRequest request)
        {
            User? user = new SelectCommand<User>().From(db.Users).WhereEquals(nameof(User.Username), request.Username).Run(db).FirstOrDefault();
            if (user == null) return new BadRequest(new ErrorResponse("Invalid credentials."));

            if (passwordHashService.Verify(user.PasswordHash, request.Password))
            {
                return new Ok(new LoginUserResponse(tokenService.GenerateToken(user)));
            }
            else
            {
                return new BadRequest(new ErrorResponse("Invalid credentials."));
            }
        }
    }
}
