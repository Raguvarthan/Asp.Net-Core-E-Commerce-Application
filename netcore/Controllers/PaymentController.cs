using Microsoft.AspNetCore.Mvc;
using Arthur_Clive.Helper;
using System;
using System.Collections;
using System.Text;
using Arthur_Clive.Data;
using PU = Arthur_Clive.Helper.PayUHelper;
using Arthur_Clive.Logger;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Examples;
using Arthur_Clive.Swagger;

namespace Arthur_Clive.Controllers
{
    /// <summary>Controller to make payment using PayUMoney and get return responce</summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        /// <summary>Make payment using PayUMoney</summary>
        /// <remarks>This api is used to get fprm for making payment using Pay U Money gateway</remarks>
        /// <param name="model">Contains the data needed to make payment</param>
        /// <response code="200">Returns the form needed to make the paymnet through PayUMoney gateway</response>
        /// <response code="400">Process ran into an exception</response> 
        [HttpPost]
        [SwaggerRequestExample(typeof(PaymentModel), typeof(PaymentDetails))]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public ActionResult MakePayment([FromBody]PaymentModel model)
        {
            try
            {
                string SuccessUrl = "http://localhost:5001/api/payment/success";
                string FailureUrl = "http://localhost:5001/api/payment/failed";
                string txnId = PU.GetTxnId();
                string hashString = PU.GetHashString(txnId,model);
                string hash = PU.Generatehash512(hashString).ToLower();
                string action = GlobalHelper.ReadXML().Elements("payu").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("url").First().Value + "/_payment";
                Hashtable data = new Hashtable();
                data.Add("hash", hash);
                data.Add("txnid", txnId);
                data.Add("key", "gtKFFx");
                string AmountForm = Convert.ToDecimal(model.Amount).ToString("g29");
                data.Add("amount", AmountForm);
                data.Add("firstname", model.FirstName);
                data.Add("email", model.Email);
                data.Add("phone", model.PhoneNumber);
                data.Add("productinfo", model.ProductInfo);
                data.Add("surl", SuccessUrl);
                data.Add("furl", FailureUrl);
                data.Add("lastname", model.LastName);
                data.Add("curl", "");
                data.Add("address1", model.AddressLine1);
                data.Add("address2", model.AddressLine2);
                data.Add("city", model.City);
                data.Add("state", model.State);
                data.Add("country", model.Country);
                data.Add("zipcode", model.ZipCode);
                data.Add("udf1", "");
                data.Add("udf2", "");
                data.Add("udf3", "");
                data.Add("udf4", "");
                data.Add("udf5", "");
                data.Add("pg", "");
                data.Add("service_provider", "PayUMoney");
                StringBuilder strForm = PU.PreparePOSTForm(action, data);
                var form = PU.PreparePOSTForm(action, data);
                dynamic UserInfo = new System.Dynamic.ExpandoObject();
                UserInfo.form = form;
                return Ok(new ResponseData
                {
                    Code = "200",
                    Content = UserInfo,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("PaymentController", "MakePayment", "MakePayment", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Get return responce for payment made
        /// </summary>
        /// <param name="form">Form returned when payment is made</param>
        /// <param name="responce">Responce received paument gateway when payment is made</param>
        /// <remarks>This api returns the responce for the payment made through Pay U Money</remarks>
        /// <response code="200">Payment is made successfully made</response>
        /// <response code="401">Payment failed</response> 
        /// <response code="400">Process ran into an exception</response> 
        [HttpPost("{responce}")]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public ActionResult Return([FromBody]FormCollection form,string responce)
        {
            try
            {
                if (form["status"].ToString() == "success")
                {
                    string[] hashSequence = ("key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10").Split('|');
                    Array.Reverse(hashSequence);
                    string hashString = GlobalHelper.ReadXML().Elements("payu").Where(x => x.Element("current").Value.Equals("Yes")).Descendants("saltkey").First().Value + "|" + form["status"].ToString();
                    foreach (string data in hashSequence)
                    {
                        hashString += "|";
                        hashString = hashString + (form[data].ToString() != null ? form[data].ToString() : "");
                    }
                    //Response.Write(merc_hash_string);
                    string hash = PU.Generatehash512(hashString).ToLower();
                    dynamic UserInfo = new System.Dynamic.ExpandoObject();
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Content = null,
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "401",
                        Message = "Payment failed",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("PaymentController", "Return", "Return", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = null
                });
            }
        }
    }
}
