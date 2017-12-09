using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizedServer.Models;
using Swashbuckle.AspNetCore.Examples;

namespace AuthorizedServer.Swagger
{
    #region TokenController

    /// <summary></summary>
    public class ParameterDetails : IExamplesProvider
    {
        /// <summary></summary>
        public object GetExamples()
        {
            return new Parameters()
            {
                username = "sample@gmail.com",
                fullname = "Sample User"
            };
        }
    }

    #endregion

    #region AuthController

    /// <summary></summary>
    public class RegisterDetails : IExamplesProvider
    {
        /// <summary></summary>
        public object GetExamples()
        {
            return new RegisterModel()
            {
                Title = "Mr",
                FullName = "Sample User",
                DialCode = "+91",
                PhoneNumber = "12341234",
                Email = "sample@gmail.com",
                Password = "asd123",
                UserLocation = "IN"
            };
        }
    }

    /// <summary></summary>
    public class LoginDetails : IExamplesProvider
    {
        /// <summary></summary>
        public object GetExamples()
        {
            return new LoginModel()
            {
                UserName = "12341234",
                Password = "asd123",
            };
        }
    }

    /// <summary></summary>
    public class ForgotPasswordDetails : IExamplesProvider
    {
        /// <summary></summary>
        public object GetExamples()
        {
            return new ForgotPasswordModel()
            {
                UserName = "12341234",
                UserLocation = "IN",
            };
        }
    }

    /// <summary></summary>
    public class ChangePassword_ForgotPassword : IExamplesProvider
    {
        /// <summary></summary>
        public object GetExamples()
        {
            return new LoginModel()
            {
                UserName = "12341234",
                Password = "qwe123",
            };
        }
    }

    /// <summary></summary>
    public class ChangePasswordDetails : IExamplesProvider
    {
        /// <summary></summary>
        public object GetExamples()
        {
            return new ChangePasswordModel()
            {
                UserName = "12341234",
                OldPassword = "asd123",
                NewPassword = "qwe123"
            };
        }
    }

    /// <summary></summary>
    public class DeactivateAccountDetails : IExamplesProvider
    {
        /// <summary></summary>
        public object GetExamples()
        {
            return new LoginModel()
            {
                UserName = "12341234",
                Password = "asd123",
            };
        }
    }

    /// <summary></summary>
    public class SocialLoginDetails : IExamplesProvider
    {
        /// <summary></summary>
        public object GetExamples()
        {
            return new SocialLoginModel()
            {
                Token = "Token received from google or facebook",
                Email = "sample@gmail.com",
                ID = "ID given by google or facebook"
            };
        }
    }

    #endregion
}
