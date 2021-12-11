using NUnit.Framework;
using Rest.Attributes;
using Rest.ResponseTypes;
using System;

namespace Rest.Test
{
    public class ControllerRegistryTests
    {
        private ControllerRegistry controllerRegistry;

        [SetUp]
        public void Setup()
        {
            controllerRegistry = new ControllerRegistry();
        }

        [Test]
        [TestCase(typeof(ValidController1))]
        [TestCase(typeof(ValidController2))]
        public void TestValidRegistration(Type type)
        {
            Assert.DoesNotThrow(() => controllerRegistry.AddController(type));
        }

        [Test]
        [TestCase(typeof(InvalidController1))]
        [TestCase(typeof(InvalidController2))]
        [TestCase(typeof(InvalidController3))]
        public void TestInvalidRegistration(Type type)
        {
            Assert.Throws<ArgumentException>(() => controllerRegistry.AddController(type));
        }

        [Test]
        public void TestGetHandler()
        {
            controllerRegistry.AddController<ValidController1>();

            HandlerInfo? getHandler = controllerRegistry.GetHandler(new TestApiRequest()
            {
                Method = Method.GET,
                Path = "/test1/abc",
                Parameters = new System.Collections.Generic.Dictionary<string, string>(),
                Content = "{ Content: 'Test' }"
            });

            HandlerInfo? postHandler = controllerRegistry.GetHandler(new TestApiRequest()
            {
                Method = Method.POST,
                Path = "/test1",
                Parameters = new System.Collections.Generic.Dictionary<string, string>(),
                Content = "{ Content: 'Test' }"
            });
        }

        [Route("/test1")]
        public class ValidController1
        {
            [Method(Method.GET)]
            public IApiResponse Get([FromBody] object request, [FromRoute] string id)
            {
                return new Ok(request);
            }

            [Method(Method.POST)]
            public IApiResponse Post([FromBody] object request)
            {
                return new Ok(request);
            }
        }

        [Route("/test2")]
        public class ValidController2
        {
            [Method(Method.GET)]
            public IApiResponse Get([FromBody] object request)
            {
                return new Ok(request);
            }

            [Method(Method.GET)]
            public IApiResponse Get2()
            {
                return new Ok();
            }
        }

        [Route("/test3")]
        public class InvalidController1
        {
            [Method(Method.GET)]
            public IApiResponse Get()
            {
                return new Ok();
            }

            [Method(Method.GET)]
            public IApiResponse Get2()
            {
                return new Ok();
            }
        }

        [Route("/test4")]
        public class InvalidController2
        {
            [Method(Method.GET)]
            public IApiResponse Get([FromRoute] object request, [FromRoute] object request2)
            {
                return new Ok();
            }
        }

        [Route("/test5")]
        public class InvalidController3
        {
            [Method(Method.GET)]
            public IApiResponse Get([FromBody] object request, [FromBody] object request2)
            {
                return new Ok();
            }
        }
    }
}
