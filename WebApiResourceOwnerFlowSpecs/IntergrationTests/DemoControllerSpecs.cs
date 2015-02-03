using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiResourceOwnerFlow;

namespace WebApiResourceOwnerFlowSpecs.IntergrationTests
{
    [TestClass]
    public class DemoControllerSpecs
    {
        [TestMethod]
        public async Task Get_without_authorization_header_should_return_unauthorized()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.GetAsync("/api/demo/");

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [TestMethod]
        public async Task Get_with_invalid_authorization_header_should_return_unauthorized()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var client = server.HttpClient;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalidtoken");

                var response = await client.GetAsync("/api/demo/");
                
                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }
        }

        [TestMethod]
        public async Task Get_with_valid_user_credentials_should_return_message()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var httpClient = server.HttpClient;
                await httpClient.AuthorizeWith("maurice", "pass");

                var response = await httpClient.GetStringAsync("/api/demo/");

                response.Should().Be("\"The user is: maurice\"");
            }
        }

        [TestMethod]
        public async Task Get_with_invalid_user_credentials_should_return_message()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var httpClient = server.HttpClient;

                await httpClient.AuthorizeWith("nouser", "nopass");

                Func<Task> request = async () => await httpClient.GetStringAsync("/api/demo/");
                request.ShouldThrow<HttpRequestException>()
                    .And.Message.Should().Contain("401 (Unauthorized)");
            }
        }
    }
}