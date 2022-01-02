using Rest.Attributes;
using Rest.ResponseTypes;

namespace Rest.Test.Intragration
{
    [Route("/echo")]
    internal class EchoController
    {
        private readonly ITestService service;

        public EchoController(ITestService service)
        {
            this.service = service;
        }

        [Method(Method.GET)]
        public IApiResponse Get([FromBody] EchoRequest request, [FromRoute] string id, [FromParameter("a")] string a, [FromParameter("b")] string b)
        {
            EchoResponse response = new EchoResponse();
            response.Content = $"{request.Content}\n{id}\n{a}\n{b}";

            return new Ok(response);
        }

        [Method(Method.GET)]
        [Restrict(Role.ADMIN)]
        public IApiResponse TestAuth()
        {
            EchoResponse response = new EchoResponse();
            response.Content = service.GetTheThing();

            return new Ok(response);
        }
    }
}
