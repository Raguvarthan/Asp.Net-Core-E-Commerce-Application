using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AuthorizedServer.Logger;

namespace AuthorizedServer.Helper
{
    /// <summary>Helper for Amazon SNS service</summary>
    public class SMSHelper
    {
        /// <summary>Get amazon SNS service credentials from xml file</summary>
        /// <param name="key"></param>
        public static string GetCredentials(string key)
        {
            var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var xmlStr = File.ReadAllText(Path.Combine(dir,"AmazonKeys.xml"));
            var str = XElement.Parse(xmlStr);
            var result = str.Elements("amazonsns").Where(x => x.Element("current").Value.Equals("Yes")).Descendants(key);
            return result.First().Value;
        }

        /// <summary>Send sms using amazon SNS service</summary>
        /// <param name="phoneNumber"></param>
        /// <param name="otp"></param>
        public static string SendSMS(string phoneNumber, string otp)
        {
            try
            {
                AmazonSimpleNotificationServiceClient smsClient = new AmazonSimpleNotificationServiceClient
                    (GetCredentials("accesskey"), GetCredentials("secretkey"), Amazon.RegionEndpoint.APSoutheast1);

                var smsAttributes = new Dictionary<string, MessageAttributeValue>();

                MessageAttributeValue senderID = new MessageAttributeValue();
                senderID.DataType = "String";
                senderID.StringValue = "ArthurClive";

                MessageAttributeValue sMSType = new MessageAttributeValue();
                sMSType.DataType = "String";
                sMSType.StringValue = "Transactional";

                MessageAttributeValue maxPrice = new MessageAttributeValue();
                maxPrice.DataType = "Number";
                maxPrice.StringValue = "0.5";

                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;

                smsAttributes.Add("AWS.SNS.SMS.SenderID", senderID);
                smsAttributes.Add("AWS.SNS.SMS.SMSType", sMSType);
                smsAttributes.Add("AWS.SNS.SMS.MaxPrice", maxPrice);

                string message = "Verification code for your Arthur Clive registration request is " + otp;

                PublishRequest publishRequest = new PublishRequest();
                publishRequest.Message = message;
                publishRequest.MessageAttributes = smsAttributes;
                publishRequest.PhoneNumber = "+91" + phoneNumber;

                Task<PublishResponse> result = smsClient.PublishAsync(publishRequest, token);
                return "Success";
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("SMSHelper", "SendSMS", "SendSMS", ex.Message);
                return "Failed";
            }
        }
    }
}
