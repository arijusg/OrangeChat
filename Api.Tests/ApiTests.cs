using System.Collections.Generic;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using System.Net.Http;

namespace Api.Tests
{
    public class ApiTests
    {
        private TestServer _server;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            _server = TestServer.Create<Api.Startup>();
        }

        [OneTimeTearDown]
        public void FixtureDispose()
        {
            _server.Dispose();
        }

        [Test]
        public void TestGetAllValues()
        {
            var response = _server.HttpClient.GetAsync("/api/values").Result;
            var result = response.Content.ReadAsAsync<IEnumerable<string>>().Result;

            CollectionAssert.AreEqual(new[] { "value1", "value2" }, result);
        }
    }
}
