using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using AuthorizedServer.Logger;
using AuthorizedServer.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MH = AuthorizedServer.Helper.MongoHelper;

namespace AuthorizedServer.Helper
{
    /// <summary>Global helper for authorized controller </summary>
    public class GlobalHelper
    {
        /// <summary>To read XML</summary>
        public static XElement ReadXML()
        {
            var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var xmlStr = File.ReadAllText(Path.Combine(dir, "AmazonKeys.xml"));
            return XElement.Parse(xmlStr);
        }

        /// <summary>Get ip config from xml</summary>
        public static string GetIpConfig()
        {
            var result = ReadXML().Elements("ipconfig").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("authorizedserver2");
            return result.First().Value;
        }

        /// <summary>To record invalid login attempts</summary>
        /// <param name="filter"></param>
        public static string RecordLoginAttempts(FilterDefinition<BsonDocument> filter)
        {
            try
            {
                var verifyUser = BsonSerializer.Deserialize<RegisterModel>(MH.GetSingleObject(filter, "Authentication", "Authentication").Result);
                if (verifyUser.WrongAttemptCount < 10)
                {
                    var update = Builders<BsonDocument>.Update.Set("WrongAttemptCount", verifyUser.WrongAttemptCount + 1);
                    var result = MH.UpdateSingleObject(filter, "Authentication", "Authentication", update).Result;
                    return "Login Attempt Recorded";
                }
                else
                {
                    var update = Builders<BsonDocument>.Update.Set("Status", "Revoked");
                    var result = MH.UpdateSingleObject(filter, "Authentication", "Authentication", update).Result;
                    return "Account Blocked";
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("AuthController", "RecordLoginAttempts", "RecordLoginAttempts", ex.Message);
                return "Failed";
            }
        }
    }
}
