    using System;
using System.Linq;
using System.Threading.Tasks;
using Arthur_Clive.Data;
using Arthur_Clive.Helper;
using Arthur_Clive.Logger;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MH = Arthur_Clive.Helper.MongoHelper;

namespace Arthur_Clive.Controllers
{
    /// <summary>Controller to subscribe and unsubscribe user and to send email to subscribed users</summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        /// <summary></summary>
        public IMongoDatabase _db = MH._client.GetDatabase("SubscribeDB");

        /// <summary>Subscribe user</summary>
        /// <param name="username">UserName of user who needs to be subscribed</param>
        /// <remarks>This api adds user to subscribed user list</remarks>
        /// <response code="200">User subscribed successfully</response>
        /// <response code="401">User already subscribed</response>    
        /// <response code="402">UserName is empty</response>
        /// <response code="400">Process ran into an exception</response>    
        [HttpPost("subscribe/{username}")]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> Subscribe(string username)
        {
            try
            {
                if (username != null)
                {
                    var user = MH.CheckForDatas("UserName", username, null, null, "SubscribeDB", "SubscribedUsers");
                    if (user == null)
                    {
                        var collection = _db.GetCollection<Subscribe>("SubscribedUsers");
                        await collection.InsertOneAsync(new Subscribe { UserName = username });
                        return Ok(new ResponseData
                        {
                            Code = "200",
                            Message = "Subscribed Succesfully",
                            Data = null
                        });
                    }
                    else
                    {
                        return BadRequest(new ResponseData
                        {
                            Code = "401",
                            Message = "User Already Subscribed",
                            Data = null
                        });
                    }
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "402",
                        Message = "UserName connot be empty",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("AdminContoller", "Subscribe", "Subscribe", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Unsubscribe user</summary>
        /// <param name="username">UserName of user who need to get unsubscribed</param>
        /// <remarks>This api removes user from subscribed user list</remarks>
        /// <response code="200">User unsubscribed successfully</response>
        /// <response code="404">User not found</response>    
        /// <response code="402">UserName is empty</response>
        /// <response code="400">Process ran into an exception</response>    
        [HttpPost("unsubscribe/{username}")]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public ActionResult Unsubscribe(string username)
        {
            try
            {
                if (username != null)
                {
                    var user = MH.CheckForDatas("UserName", username, null, null, "SubscribeDB", "SubscribedUsers");
                    if (user != null)
                    {
                        var filter = Builders<BsonDocument>.Filter.Eq("UserName", username);
                        MH.DeleteSingleObject(filter, "SubscribeDB", "SubscribedUsers");
                        return Ok(new ResponseData
                        {
                            Code = "200",
                            Message = "Unsubscribed Succesfully",
                            Data = null
                        });
                    }
                    else
                    {
                        return BadRequest(new ResponseData
                        {
                            Code = "404",
                            Message = "No user found",
                            Data = null
                        });
                    }
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "402",
                        Message = "UserName connot be empty",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("AdminContoller", "Unsubscribe", "Unsubscribe", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Send email to all subscribed users</summary>
        /// <remarks>This api sends message to all subscribed users</remarks>
        /// <param name="message">Message that is to be sent throungh email to the subscribed users by admin</param>
        /// <response code="200">Email successfully sent to all subscribed users</response>
        /// <response code="404">No user found to be subscribed</response> 
        /// <response code="400">Process ran into an exception</response>    
        [HttpPost("sendmessage")]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> PublicPost([FromBody]string message)
        {
            try
            {
                var collection = _db.GetCollection<Subscribe>("SubscribedUsers");
                var filter = FilterDefinition<Subscribe>.Empty;
                IAsyncCursor<Subscribe> cursor = await collection.FindAsync(filter);
                var users = cursor.ToList();
                if (users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        await EmailHelper.SendEmail(user.UserName, user.UserName, message);
                    }
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Message = "Email sent to all subscribed users",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "404",
                        Message = "There are no subscribed users",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("AdminContoller", "PublicPost", "PublicPost", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }
    }
}
