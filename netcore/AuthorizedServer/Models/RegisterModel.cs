using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace AuthorizedServer.Models
{
    /// <summary>Contains registration details of the user </summary>
    public class RegisterModel
    {
        /// <summary>ObjectId given by MongoDB</summary>
        public ObjectId _id { get; set; }
        /// <summary>Titile of the user</summary>
        [Required]
        public string Title { get; set; }
        /// <summary>Fullname of user</summary>
        [Required]
        public string FullName { get; set; }
        /// <summary>Username of user</summary>
        public string UserName { get; set; }
        /// <summary>Dialcode for phonenumber of the user</summary>
        [Required]
        public string DialCode { get; set; }
        /// <summary>Phonenumber of the user</summary>
        [Required]
        public string PhoneNumber { get; set; }
        /// <summary>Email of the user</summary>
        [Required]
        public string Email { get; set; }
        /// <summary>SocialId of user incase the user login using facebook and gmail</summary>
        [Required]
        public string SocialId { get; set; }
        /// <summary>Password of the user</summary>
        [Required]
        public string Password { get; set; }
        /// <summary>Location of the user</summary>
        [Required]
        public string UserLocation { get; set; }
        /// <summary>Verification code sent ot the user</summary>
        public string VerificationCode { get; set; }
        /// <summary>Status of the user</summary>
        public string Status { get; set; }
        /// <summary>Count of invalid login attempts</summary>
        public int WrongAttemptCount { get; set; }
        /// <summary>OTP expiration time</summary>
        public DateTime OTPExp { get; set; }
    }

    /// <summary>Contains details required to change password when the user forgets the current password</summary>
    public class ForgotPasswordModel
    {
        /// <summary>UserName of user</summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>Location of user</summary>
        [Required]
        public string UserLocation { get; set; }
        /// <summary>Verification code sent to the user</summary>
        public string VerificationCode { get; set; }
        /// <summary>Verification status of user</summary>
        public string Status { get; set; }
        /// <summary>Expiration time for otp</summary>
        public DateTime OTPExp { get; set; }
    }

    /// <summary>Contains data needs to verify user</summary>
    public class VerificationModel
    {
        /// <summary>Username of user</summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>Vrification code sent to user</summary>
        [Required]
        public string VerificationCode { get; set; }
    }

    /// <summary>Contains data need to login user</summary>
    public class LoginModel
    {
        /// <summary>Username of user</summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>Password od user</summary>
        [Required]
        public string Password { get; set; }
    }

    /// <summary>Contains deatils need to change password </summary>
    public class ChangePasswordModel
    {
        /// <summary>Username of user</summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>Currnt password of the user</summary>
        [Required]
        public string OldPassword { get; set; }
        /// <summary>New password to be replace the current password</summary>
        [Required]
        public string NewPassword { get; set; }
    }

    /// <summary>Contails datas for social login</summary>
    public class SocialLoginModel
    {
        /// <summary>Token received from google or facebook</summary>
        [Required]
        public string Token { get; set; }
        /// <summary>Email of user</summary>
        [Required]
        public string Email { get; set; }
        /// <summary>Unique id received by the user from facebook or google</summary>
        [Required]
        public string ID { get; set; }
    }
    
    /// <summary>Contains data obtained from google token</summary>
    public class GoogleVerificationModel
    {
        /// <summary>Name of user received from google</summary>
        [Required]
        public string name { get; set; }
        /// <summary>Email of user received from google</summary>
        [Required]
        public string email { get; set; }
        /// <summary>Boolean that defines weather the user id vewrified ot not in google</summary>
        [Required]
        public bool email_verified { get; set; }
        /// <summary></summary>
        public string azp { get; set; }
        /// <summary></summary>
        public string aud { get; set; }
        /// <summary>Unique ID given by google to user</summary>
        [Required]
        public string sub { get; set; }
        /// <summary></summary>
        public string at_hash { get; set; }
        /// <summary></summary>
        public string iss { get; set; }
        /// <summary></summary>
        public string jti { get; set; }
        /// <summary></summary>
        public string iat { get; set; }
        /// <summary></summary>
        public string exp { get; set; }
        /// <summary>Url that contain profile picture of user given by google</summary>
        public string picture { get; set; }
        /// <summary></summary>
        public string given_name { get; set; }
        /// <summary></summary>
        public string family_name { get; set; }
        /// <summary></summary>
        public string locale { get; set; }
        /// <summary></summary>
        public string alg { get; set; }
        /// <summary></summary>
        public string kid { get; set; }
    }

    /// <summary>Contains data obtained from facebook token</summary>
    public class FacebookVerificationModel
    {
        /// <summary>Unique id given by facebook</summary>
        [Required]
        public string id { get; set; }
        /// <summary>Name of the used obtained from the facebook token</summary>
        [Required]
        public string name { get; set; }
    }
}
