using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Arthur_Clive.Helper
{
    /// <summary>Contains to make payment through PayUMoney</summary>
    public class PaymentModel
    {
        /// <summary>FirstName of user</summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>LastName of user</summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>ProductInfo of product from which the payment is made</summary>
        [Required]
        public string ProductInfo { get; set; }
        /// <summary>Amount to be paid for order</summary>
        [Required]
        public string Amount { get; set; }
        /// <summary>Email of user</summary>
        [Required]
        public string Email { get; set; }
        /// <summary>PhoneNumber of user</summary>
        [Required]
        public string PhoneNumber { get; set; }
        /// <summary>First line of address </summary>
        [Required]
        public string AddressLine1 { get; set; }
        /// <summary>Secound line of address</summary>
        [Required]
        public string AddressLine2 { get; set; }
        /// <summary>City of user</summary>
        [Required]
        public string City { get; set; }
        /// <summary>State of user </summary>
        [Required]
        public string State { get; set; }
        /// <summary>Country of user </summary>
        [Required]
        public string Country { get; set; }
        /// <summary>Zipcode of user location</summary>
        [Required]
        public string ZipCode { get; set; }
    }

    /// <summary>Helper method for PayUMoney service</summary>
    public class PayUHelper
    {
        /// <summary>Get hash value of random number</summary>
        /// <param name="text"></param>
        public static string Generatehash512(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        /// <summary>/// Prepare post form for paymet through PayUMoney</summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public static StringBuilder PreparePOSTForm(string url, Hashtable data)
        {
            string formID = "PostForm";
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
            foreach (DictionaryEntry key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key.Key + "\" value=\"" + key.Value + "\">");
            }
            strForm.Append("</form>");
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            strForm.Append(strScript);
            return strForm;
        }

        /// <summary>Get TxnId</summary>
        public static string GetTxnId()
        {
            Random random = new Random();
            string strHash = Generatehash512(random.ToString() + DateTime.Now);
            string txnId = strHash.ToString().Substring(0, 20);
            return txnId;
        }
        
        /// <summary>Get hash string</summary>
        /// <param name="txnId"></param>
        /// <param name="model"></param>
        public static string GetHashString(string txnId,PaymentModel model)
        {
            string hashString = "";
            string[] hashSequence = ("key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10").Split('|');
            foreach (string hash_var in hashSequence)
            {
                if (hash_var == "key")
                {
                    hashString = hashString + GlobalHelper.ReadXML().Elements("payu").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("key").First().Value;
                    hashString = hashString + '|';
                }
                else if (hash_var == "txnid")
                {
                    hashString = hashString + txnId;
                    hashString = hashString + '|';
                }
                else if (hash_var == "amount")
                {
                    hashString = hashString + Convert.ToDecimal(model.Amount).ToString("g29");
                    hashString = hashString + '|';
                }
                else if (hash_var == "productinfo")
                {
                    hashString = hashString + model.ProductInfo;
                    hashString = hashString + '|';
                }
                else if (hash_var == "firstname")
                {
                    hashString = hashString + model.FirstName;
                    hashString = hashString + '|';
                }
                else if (hash_var == "email")
                {
                    hashString = hashString + model.Email;
                    hashString = hashString + '|';
                }
                else
                {
                    hashString = hashString + "";
                    hashString = hashString + '|';
                }
            }
            hashString += GlobalHelper.ReadXML().Elements("payu").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("saltkey").First().Value;
            return hashString;
        }
    }
}
