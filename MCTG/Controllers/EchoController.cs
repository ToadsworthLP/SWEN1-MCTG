using MCTG.Auth;
using MCTG.Requests;
using MCTG.Responses;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/echo")]
    internal class EchoController
    {
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
            response.Content = $"Without route\n{request.Content}\n{a}\n{b}";

            return new Ok(response);
        }

        [Method(Method.GET)]
        [Restrict(Role.ADMIN)]
        public IApiResponse Test()
        {
            EchoResponse response = new EchoResponse();
            response.Content = "Hello, Admin!";

            return new Ok(response);
        }
    }
}
