using NUnit.Framework;
using Rest.Test.Intragration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Rest.Test
{
    public class FullServerTest
    {
        [Test]
        public void TestGet()
        {
            Task server = Task.Run(() =>
            {
                RestServer server = new RestServer(System.Net.IPAddress.Loopback, 3000);
                server.AddController<EchoController>();
                server.AddScoped<ITestService, TestService>();
                server.Start();
            });

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:3000/echo/some-id?a=test")
            {
                Content = JsonContent.Create(new EchoRequest() { Content = "Hello World!" })
            };
            request.Content.Headers.ContentLength = 31;

            HttpResponseMessage response = httpClient.Send(request);

            Assert.IsTrue(response.IsSuccessStatusCode);

            string responseBody = response.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(responseBody == "{\"Content\":\"Hello World!\\nsome-id\\ntest\\n\"}");
        }

        [Test]
        public void TestAuth()
        {
            Task server = Task.Run(() =>
            {
                RestServer server = new RestServer(System.Net.IPAddress.Loopback, 3000);
                server.AddController<EchoController>();
                server.AddScoped<ITestService, TestService>();
                server.AddAuth<AuthProvider>();
                server.Start();
            });

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:3000/echo/");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "admin");

            HttpResponseMessage response = httpClient.Send(request);

            Assert.IsTrue(response.IsSuccessStatusCode);

            string responseBody = response.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(responseBody == "{\"Content\":\"Calls to same instance: 0\"}");
        }
    }
}
