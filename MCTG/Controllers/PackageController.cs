using MCTG.Config;
using MCTG.Requests;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/packages")]
    internal class PackageController
    {
        private readonly AppDbContext db;

        public PackageController(AppDbContext db)
        {
            this.db = db;
        }

        [Method(Method.POST)]
        public IApiResponse AddPackage([FromBody] AddPackageRequest[] requests)
        {
            return new Ok(requests.Length);
        }
    }
}
