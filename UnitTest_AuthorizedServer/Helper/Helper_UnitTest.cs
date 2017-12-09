using AuthorizedServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthorizedServer.Helper;
using AuthorizedServer.Repositories;
using Microsoft.Extensions.Options;
using Moq;
using SH = AuthorizedServer.Helper.SMSHelper;
using System;

namespace UnitTest_AuthorizedServer.Helper
{
    [TestClass]
    public class AuthHelper_Test
    {
        public AuthHelper authHelper = new AuthHelper();

        [TestMethod]
        public void DoPassword()
        {
            //Arrange
            var repo = new Mock<IRTokenRepository>();
            var settings = new Mock<IOptions<Audience>>();
            Parameters parameters = new Parameters();
            parameters.username = "sample@gmail.com";
            parameters.fullname = "Sample User";

            //Act
            var result = authHelper.DoPassword(parameters, repo.Object, settings.Object);

            //Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual("999", result.Code);
            //Assert.IsNotNull(result.Data);
            //Assert.AreEqual("Ok", result.Message);
        }

        [TestMethod]
        public void DoRefreshToken()
        {
            //Arrange
            var repo = new Mock<IRTokenRepository>();
            var settings = new Mock<IOptions<Audience>>();
            Parameters parameters = new Parameters();
            parameters.username = "sample@gmail.com";
            parameters.fullname = "Sample User";

            //Act
            var result = authHelper.DoRefreshToken(parameters, repo.Object, settings.Object);

            //Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual("999", result.Code);
            //Assert.IsNotNull(result.Data);
            //Assert.AreEqual("Ok", result.Message);
        }

        [TestMethod]
        public void GetJWT()
        {
            //Arrange
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var client_id = "sample@gmail.com";
            var settings = new Mock<IOptions<Audience>>();

            //Act
            var result = authHelper.GetJwt(client_id, refresh_token, settings.Object) as string;

            //Assert
            Assert.IsNotNull(result);
        }
    }

    [TestClass]
    public class SMSHelper_Test
    {
        [TestMethod]
        public void GetCredentials()
        {
            //Arrange
            var key1 = "accesskey";
            var key2 = "secretkey";

            //Act
            var result1 = SH.GetCredentials(key1) as string;
            var result2 = SH.GetCredentials(key2) as string;

            //Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void SendSMS()
        {
            //Arrange
            var phoneNumber = "12341234";
            var otp = "123456";

            //Act
            var result = SH.SendSMS(phoneNumber,otp) as string;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Success",result);
        }
    }
}
