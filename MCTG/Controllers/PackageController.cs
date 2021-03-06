using MCTG.Auth;
using MCTG.Config;
using MCTG.Models;
using MCTG.Requests;
using MCTG.Responses;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/packages")]
    public class PackageController
    {
        private readonly AppDbContext db;

        public PackageController(AppDbContext db)
        {
            this.db = db;
        }

        [Method(Method.POST)]
        [Restrict(Role.ADMIN)]
        public IApiResponse AddPackage([FromBody] AddPackageRequest[] requests)
        {
            if (requests.Length == 0) return new BadRequest(new ErrorResponse("Cannot create an empty package."));

            Package package = db.Packages.Create(new Package(Constants.DEFAULT_PACKAGE_PRICE));

            foreach (AddPackageRequest request in requests)
            {
                Card card;

                if (request.Id == null)
                {
                    card = db.Cards.Create(new Card(request.Name, request.Damage, request.Type));
                }
                else
                {
                    card = db.Cards.Create(new Card((Guid)request.Id, request.Name, request.Damage, request.Type));
                }

                db.PackageEntries.Create(new PackageEntry(package.Id, card.Id));
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
    }
}
