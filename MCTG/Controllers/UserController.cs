using Data.SQL;
using MCTG.Auth;
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

        [Method(Method.GET)]
        [Restrict(Role.USER)]
        public IApiResponse ViewProfile([FromRoute] string username)
        {
            User? target = new SelectCommand<User>().From(db.Users).WhereEquals(nameof(User.Username), username).Run(db).FirstOrDefault();
            if (target != null && AuthProvider.CurrentUser != null && target.Id == AuthProvider.CurrentUser.Id)
            {
                return new Ok(new UserProfileResponse(target.Username, target.Bio, target.Image, target.Elo));
            }
            else
            {
                return new NotFound();
            }
        }

        [Method(Method.POST)]
        public IApiResponse Register([FromBody] RegisterUserRequest request)
        {
            if (request.Username == "admin") // Make the user named "admin" an administrator upon registration - needed to match the specs
            {
                db.Users.Create(new User(request.Username, passwordHashService.Hash(request.Password), Auth.Role.ADMIN));
            }
            else
            {
                db.Users.Create(new User(request.Username, passwordHashService.Hash(request.Password), Auth.Role.USER));
            }

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

        [Method(Method.PUT)]
        [Restrict(Role.USER)]
        public IApiResponse Edit([FromBody] EditUserRequest request, [FromRoute] string username)
        {
            User? target = new SelectCommand<User>().From(db.Users).WhereEquals(nameof(User.Username), username).Run(db).FirstOrDefault();
            if (target != null && AuthProvider.CurrentUser != null && target.Id == AuthProvider.CurrentUser.Id)
            {
                User updated = target with { Username = request.Name, Bio = request.Bio, Image = request.Image };
                db.Users.Update(updated);

                try
                {
                    db.Commit();
                    return new Ok();
                }
                catch
                {
                    return new BadRequest(new ErrorResponse("One or more of the provided values are invalid."));
                }
            }
            else
            {
                return new NotFound();
            }
        }
    }
}
