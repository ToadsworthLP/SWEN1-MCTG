using NUnit.Framework;
using Rest.Http;
using System;
using System.IO;
using System.Text;

namespace Rest.Test
{
    public class Tests
    {
        [Test]
        [TestCase("GET /test HTTP/1.1\nContent-Length: 0", Method.GET, "/test", "")]
        [TestCase("GET /test HTTP/1.1\nContent-Length: 29\n\n{ 'Content': 'Hello World!' }", Method.GET, "/test", "{ 'Content': 'Hello World!' }")]
        [TestCase("GET /test/ HTTP/1.1\nContent-Length: 29\n\n{ 'Content': 'Hello World!' }", Method.GET, "/test", "{ 'Content': 'Hello World!' }")]
        [TestCase("POST /test HTTP/1.1\nContent-Length: 26\n\n{ 'Content': 'Some data' }", Method.POST, "/test", "{ 'Content': 'Some data' }")]
        [Parallelizable(ParallelScope.All)]
        public void ParseValidHttpRequest(string request, Method method, string path, string content)
        {
            StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(request)));

            IApiRequest parsedRequest;
            parsedRequest = new HttpRequest(reader);

            Assert.IsTrue(parsedRequest.Method == method);
            Assert.IsTrue(parsedRequest.Path == path);
            Assert.IsTrue(parsedRequest.Content == content);
        }

        [Test]
        [TestCase("GET /test HTTP/1.1\nContent-Length: 0", null, null)]
        [TestCase("GET /test?a HTTP/1.1\nContent-Length: 0", "a", "")]
        [TestCase("GET /test?a=b HTTP/1.1\nContent-Length: 0", "a", "b")]
        [TestCase("GET /test?a=%C3%A4%C3%BC%C3%B6%C3%9F HTTP/1.1\nContent-Length: 0", "a", "δόφί")]
        [Parallelizable(ParallelScope.All)]
        public void TestQueryParameters(string request, string? key, string? value)
        {
            StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(request)));

            IApiRequest parsedRequest;
            parsedRequest = new HttpRequest(reader);

            if (key != null && value != null)
            {
                Assert.IsTrue(parsedRequest.Parameters[key] == value);
            }
            else
            {
                Assert.IsTrue(parsedRequest.Parameters.Count == 0);
            }
        }

        [Test]
        public void TestAuthToken()
        {
            StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes("GET /test HTTP/1.1\nAuthorization: Basic admin\nContent-Length: 0")));

            IApiRequest parsedRequest;
            parsedRequest = new HttpRequest(reader);

            Assert.IsTrue(parsedRequest.AuthToken == "admin");
        }

        [Test]
        [TestCase("")]
        [TestCase("aaaaaaaaaaaaaaaa")]
        [TestCase("GET\nbbbbbbbbbbbbbbbb")]
        [TestCase("xxx /test HTTP/1.1\nContent-Length: 0")]
        [Parallelizable(ParallelScope.All)]
        public void ParseInvalidHttpRequest(string request)
        {
            StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(request)));

            IApiRequest parsedRequest;
            Assert.Throws<ArgumentException>(() => parsedRequest = new HttpRequest(reader));
        }
    }
}