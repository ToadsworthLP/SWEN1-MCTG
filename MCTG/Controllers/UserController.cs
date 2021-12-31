using MCTG.Config;
using MCTG.Models;
using MCTG.Requests;
using MCTG.Services;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/users")]
    internal class UserController
    {
        private readonly AppDbContext db;
        private readonly IPasswordHashService passwordHashService;

        public UserController(AppDbContext db, IPasswordHashService passwordHashService)
        {
            this.db = db;
            this.passwordHashService = passwordHashService;
        }

        [Method(Method.POST)]
        public IApiResponse Register([FromBody] RegisterUserRequest request)
        {
            db.Users.Create(new User(request.Username, passwordHashService.Hash(request.Password)));

            try
            {
                db.Commit();
                return new Ok();
            }
            catch
            {
                return new BadRequest();
            }
        }
    }
}
