using MCTG.Auth;
using MCTG.Requests;
using MCTG.Responses;
using MCTG.Services;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/echo")]
    internal class EchoController
    {
        private readonly ITestService service;
        private readonly AuthProvider authProvider;

        public EchoController(ITestService service, ITestService service2, AuthProvider authProvider)
        {
            this.service = service;
            this.authProvider = authProvider;
        }

        [Method(Method.GET)]
        public IApiResponse Get([FromBody] EchoRequest request, [FromRoute] string id, [FromParameter("a")] string a, [FromParameter("b")] string b)
        {
            EchoResponse response = new EchoResponse();
            response.Content = $"{request.Content}\n{id}\n{a}\n{b}";

            return new Ok(response);
        }

        [Method(Method.GET)]
        public IApiResponse Get([FromBody] EchoRequest request, [FromParameter("a")] string a, [FromParameter("b")] string b)
        {
            EchoResponse response = new EchoResponse();
            response.Content = service.GetTheThing();

            return new Ok(response);
        }

        [Method(Method.GET)]
        [Restrict(Role.USER)]
        public IApiResponse Test()
        {
            return new Ok(authProvider.currentRole);
        }
    }
}
