using MCTG.Requests;
using MCTG.Responses;
using Rest;
using Rest.Attributes;
using Rest.ResponseTypes;

namespace MCTG.Controllers
{
    [Route("/echo")]
    internal class EchoController : IGet<EchoRequest>
    {
        public IApiResponse Get(EchoRequest request)
        {
            EchoResponse response = new EchoResponse();
            response.Content = request.Content;

            return new Ok(response);
        }
    }
}
