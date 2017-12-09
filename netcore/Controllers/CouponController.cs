using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arthur_Clive.Data;
using Arthur_Clive.Logger;
using Arthur_Clive.Swagger;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Examples;
using MH = Arthur_Clive.Helper.MongoHelper;

namespace Arthur_Clive.Controllers
{
    /// <summary>Controller to handle request based on coupon</summary>
    [Route("api/[controller]")]
    public class CouponController : Controller
    {
        /// <summary></summary>
        public IMongoDatabase _db = MH._client.GetDatabase("CouponDB");

        /// <summary>Insert coupon</summary>
        /// <remarks>This api is used to insert a new coupon</remarks>
        /// <param name="data">Coupon data to be inserted</param>
        /// <response code="200">Coupon inserted successfully</response>
        /// <response code="401">Coupon already added</response>  
        /// <response code="400">Process ran into an exception</response>  
        [HttpPost]
        [SwaggerRequestExample(typeof(Coupon), typeof(CouponData))]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> InsertCoupon([FromBody]Coupon data)
        {
            try
            {
                var checkData = MH.CheckForDatas("Code", data.Code, null, null, "CouponDB", "Coupon");
                if (checkData == null)
                {
                    var collection = _db.GetCollection<Coupon>("Coupon");
                    await collection.InsertOneAsync(data);
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Message = "Coupon inserted successfully",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "401",
                        Message = "Coupon already added",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("CouponController", "InsertCoupon", "InsertCoupon", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Check if coupon is valid or not</summary>
        /// <remarks>This api is used to check if the coupon is valid</remarks>
        /// <param name="username">UserName of username</param>
        /// <param name="code">Coupon code</param>
        /// <response code="200">Coupon is valid</response>
        /// <response code="401">Coupon expired</response>  
        /// <response code="402">Coupon invalid</response> 
        /// <response code="403">Coupon invalid</response> 
        /// <response code="404">Coupon not found</response>  
        /// <response code="400">Process ran into an exception</response> 
        [HttpGet("check/{username}/{code}")]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public ActionResult CheckCoupon(string username, string code)
        {
            try
            {
                var checkData = MH.CheckForDatas("Code", code, null, null, "CouponDB", "Coupon");
                if (checkData != null)
                {
                    var data = BsonSerializer.Deserialize<Coupon>(checkData);
                    if (data.ExpiryTime > DateTime.UtcNow)
                    {
                        if (data.ApplicableFor == "All" || data.ApplicableFor == username )
                        {
                            if (data.UsageCount != 0)
                            {
                                return Ok(new ResponseData
                                {
                                    Code = "200",
                                    Message = "Coupon is valid",
                                    Data = null
                                });
                            }
                            else
                            {
                                return BadRequest(new ResponseData
                                {
                                    Code = "403",
                                    Message = "Coupon usage limit exceeded",
                                    Data = null
                                });
                            }
                        }
                        else
                        {
                            return BadRequest(new ResponseData
                            {
                                Code = "402",
                                Message = "Coupon invalid",
                                Data = null
                            });
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseData
                        {
                            Code = "401",
                            Message = "Coupon expired",
                            Data = null
                        });
                    }
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "404",
                        Message = "Coupon not found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("CouponController", "CheckCoupon", "CheckCoupon", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Update coupon</summary>
        /// <remarks>This api is used to update coupon details</remarks>
        /// <param name="data">Coupon details</param>
        /// <param name="code">Coupon code</param>
        /// <response code="200">Coupon updated successfully</response>
        /// <response code="404">Coupon not found</response>  
        /// <response code="400">Process ran into an exception</response> 
        [HttpPut("{code}")]
        [SwaggerRequestExample(typeof(Coupon), typeof(CouponUpdateData))]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public ActionResult UpdateCoupon([FromBody]Coupon data, string code)
        {
            try
            {
                var checkData = MH.CheckForDatas("Code", code, null, null, "CouponDB", "Coupon");
                if (checkData != null)
                {
                    var result = BsonSerializer.Deserialize<Coupon>(checkData);
                    var filter = Builders<BsonDocument>.Filter.Eq("Code", code);
                    if (data.ApplicableFor != null)
                    {
                        var update = MH.UpdateSingleObject(filter,"CouponDB","Coupon", Builders<BsonDocument>.Update.Set("ApplicableFor", data.ApplicableFor));
                    }
                    if (data.ExpiryTime != null)
                    {
                        var update = MH.UpdateSingleObject(filter, "CouponDB", "Coupon", Builders<BsonDocument>.Update.Set("ExpiryTime", data.ExpiryTime));
                    }
                    if (data.UsageCount > 0)
                    {
                        var update = MH.UpdateSingleObject(filter, "CouponDB", "Coupon", Builders<BsonDocument>.Update.Set("UsageCount", result.UsageCount - data.UsageCount));
                    }
                    if (data.Value > 0)
                    {
                        var update = MH.UpdateSingleObject(filter, "CouponDB", "Coupon", Builders<BsonDocument>.Update.Set("Value", data.Value));
                    }
                    if (data.Percentage != null)
                    {
                        var updateResult = MH.UpdateSingleObject(filter, "CouponDB", "Coupon", Builders<BsonDocument>.Update.Set("Percentage", data.Percentage));
                    }
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Message = "Coupon updated successfully",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "404",
                        Message = "Coupon not found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("CouponController", "UpdateCoupon", "UpdateCoupon", ex.Message);
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
