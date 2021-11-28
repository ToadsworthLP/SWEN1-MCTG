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
        public IApiResponse Test()
        {
            EchoResponse response = new EchoResponse();
            response.Content = "Hello World";

            return new Ok(response);
        }
    }
}
