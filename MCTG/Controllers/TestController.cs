using MCTG.Config;
using MCTG.Models;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/test")]
    internal class TestController
    {
        private readonly AppDbContext db;

        public TestController(AppDbContext db)
        {
            this.db = db;
        }

        [Method(Method.POST)]
        public IApiResponse Create()
        {
            TestModel testModel = db.TestModels.Create(new TestModel("Test", 0));

            return new Ok(testModel);
        }

        [Method(Method.POST)]
        public IApiResponse Increment([FromRoute] string id)
        {
            TestModel testModel = db.TestModels.Get(new Guid(id));
            db.TestModels.Update(testModel with { Count = testModel.Count + 1 });
            db.Commit();

            return new Ok(testModel);
        }
    }
}
