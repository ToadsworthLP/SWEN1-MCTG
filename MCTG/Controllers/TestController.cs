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
            db.Commit();

            return new Ok(testModel);
        }

        [Method(Method.POST)]
        public IApiResponse Increment([FromRoute] string id)
        {
            TestModel? testModel = db.TestModels.Get(new Guid(id));
            if (testModel != null)
            {
                TestModel updated = testModel with { Count = testModel.Count + 1 };
                db.TestModels.Update(updated);
                db.Commit();
                return new Ok(updated);
            }
            else
            {
                return new NotFound();
            }
        }

        [Method(Method.DELETE)]
        public IApiResponse Delete([FromRoute] string id)
        {
            TestModel? testModel = db.TestModels.Get(new Guid(id));
            if (testModel != null)
            {
                db.TestModels.Delete(testModel);
                db.Commit();
                return new Ok();
            }
            else
            {
                return new NotFound();
            }
        }
    }
}
