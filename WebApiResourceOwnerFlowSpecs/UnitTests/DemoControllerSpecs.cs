using System.Security.Claims;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiResourceOwnerFlow;

namespace WebApiResourceOwnerFlowSpecs.UnitTests
{
    [TestClass]
    public class DemoControllerSpecs
    {
        [TestMethod]
        public void Get_should_return_anonymous()
        {
            var controller = new DemoController
            {
                User = new ClaimsPrincipal()
            };

            var response = controller.Get();

            response.Should().Be(@"The user is: anonymous");
        }

        [TestMethod]
        public void Get_should_return_maurice()
        {
            var controller = new DemoController
            {
                User = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "Maurice")
                    }))
            };

            var response = controller.Get();

            response.Should().Be(@"The user is: Maurice");
        }
    }
}