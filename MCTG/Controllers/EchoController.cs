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
        public IApiResponse Get(EchoRequest request)
        {
            EchoResponse response = new EchoResponse();
            response.Content = request.Content;

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
