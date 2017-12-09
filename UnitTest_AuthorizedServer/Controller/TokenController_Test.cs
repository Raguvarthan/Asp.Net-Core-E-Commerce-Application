using AuthorizedServer;
using AuthorizedServer.Controllers;
using AuthorizedServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace UnitTest_AuthorizedServer
{
    [TestClass]
    public class TokenController_Test
    {
        [TestMethod]
        public void Auth(IOptions<Audience> settings, IRTokenRepository repo)
        {
            //Arrage
            Parameters parameters = new Parameters();
            parameters.grant_type = "password";
            parameters.username = "sample@gmail.com";
            parameters.fullname = "Sample User";
            TokenController controller = new TokenController(settings,repo);

            //Act
            var result = controller.Auth(parameters) as ActionResult;
            var viewResult = result.ToBsonDocument();
            var filter = viewResult["Value"].AsBsonDocument;
            var code = filter["Code"].AsString;
            var message = filter["Message"].AsString;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("999", code);

        }
    }
}
