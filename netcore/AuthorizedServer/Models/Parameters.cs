using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthorizedServer
{
    /// <summary>Contains data sent through responce</summary>
    public class ResponseData
    {
        /// <summary>Reaponce code of the responce</summary>
        [Required]
        public string Code { get; set; }
        /// <summary>Message of the responce</summary>
        public string Message { get; set; }
        /// <summary>Data of responce</summary>
        public object Data { get; set; }
        /// <summary>Other contents for responce</summary>
        public object Content { get; set; }
    }

    /// <summary>Contains data to get jwt</summary>
    public class Audience
    {
        /// <summary></summary>
        public string Secret { get; set; }
        /// <summary></summary>
        public string Iss { get; set; }
        /// <summary></summary>
        public string Aud { get; set; }
    }

    /// <summary>Contains parameters to be sent through responce</summary>
    public class Parameters
    {
        /// <summary></summary>
        public string grant_type { get; set; }
        /// <summary></summary>
        public string refresh_token { get; set; }
        /// <summary></summary>
        public string client_id { get; set; }
        /// <summary></summary>
        public string client_secret { get; set; }
        /// <summary>Username of user who get the jwt</summary>
        public string username { get; set; }
        /// <summary>Password of user who get jwt</summary>
        public string password { get; set; }
        /// <summary>Fullname of user who get jwt</summary>
        public string fullname { get; set; }
    }

    /// <summary>Contains datas regrading RToken</summary>
    public class RToken
    {
        /// <summary></summary>
        public string Id { get; set; }
        /// <summary></summary>
        [BsonElement("client_id")]
        public string ClientId { get; set; }
        /// <summary></summary>
        [BsonElement("refresh_token")]
        public string RefreshToken { get; set; }
        /// <summary></summary>
        [BsonElement("isstop")]
        public int IsStop { get; set; }
    }
}
