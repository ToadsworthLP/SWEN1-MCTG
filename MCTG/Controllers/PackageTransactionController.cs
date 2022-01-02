using Data.SQL;
using MCTG.Auth;
using MCTG.Config;
using MCTG.Models;
using MCTG.Responses;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/transactions/packages")]
    internal class PackageTransactionController
    {
        private readonly AppDbContext db;

        public PackageTransactionController(AppDbContext db)
        {
            this.db = db;
        }

        [Method(Method.POST)]
        [Restrict(Role.USER)]
        public IApiResponse BuyPackage()
        {
            if (AuthProvider.CurrentUser == null) return new BadRequest(new ErrorResponse("Not logged in."));

            Package? packageToBuy = new SelectCommand<Package>().From(db.Packages).Limit(1).Run(db).FirstOrDefault();

            if (packageToBuy == null) return new NotFound(new ErrorResponse("No packages are currently available for purchase."));

            if (packageToBuy.Price > AuthProvider.CurrentUser.Coins) return new BadRequest(new ErrorResponse("Insufficient coins for transaction."));

            IEnumerable<PackageEntry> content = new SelectCommand<PackageEntry>().From(db.PackageEntries).WhereEquals(nameof(PackageEntry.Package), packageToBuy.Id).Run(db);

            foreach (PackageEntry entry in content)
            {
                Card card = db.Cards.Get(entry.Card);
                db.Cards.Update(card with { Owner = AuthProvider.CurrentUser.Id });

                db.PackageEntries.Delete(entry);
            }

            db.Users.Update(AuthProvider.CurrentUser with { Coins = AuthProvider.CurrentUser.Coins - packageToBuy.Price });
            db.Packages.Delete(packageToBuy);

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
