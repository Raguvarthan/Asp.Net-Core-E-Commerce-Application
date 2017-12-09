using System;
using System.Threading.Tasks;
using Arthur_Clive.Data;
using Arthur_Clive.Logger;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using AH = Arthur_Clive.Helper.AmazonHelper;
using WH = Arthur_Clive.Helper.MinioHelper;
using MH = Arthur_Clive.Helper.MongoHelper;
using GH = Arthur_Clive.Helper.GlobalHelper;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Examples;
using Arthur_Clive.Swagger;
using MongoDB.Bson.Serialization;

namespace Arthur_Clive.Controllers
{
    /// <summary>Controller to get, post and delete product category</summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        /// <summary></summary>
        public IMongoDatabase _db = MH._client.GetDatabase("ProductDB");

        /// <summary>Get categories</summary>
        /// <remarks>This api gets all the categories</remarks>
        /// <response code="200">Returns categories</response>
        /// <response code="404">No categories found</response> 
        /// <response code="400">Process ran into an exception</response>  
        [HttpGet]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> Get()
        {
            try
            {
                var collection = _db.GetCollection<Category>("Category");
                var filter = FilterDefinition<Category>.Empty;
                IAsyncCursor<Category> cursor = await collection.FindAsync(filter);
                var categories = cursor.ToList();
                if (categories.Count > 0)
                {
                    foreach (var category in categories)
                    {
                        string objectName = category.ProductFor + "-" + category.ProductType + ".jpg";
                        //category.MinioObject_URL = WH.GetMinioObject("products", objectName).Result;
                        //category.MinioObject_URL = AH.GetAmazonS3Object("product-category", objectName);
                        category.MinioObject_URL = AH.GetS3Object("product-category", objectName);
                    }
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Message = "Success",
                        Data = categories
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "404",
                        Message = "No categories found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("CategoryController", "Get", "Get", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Insert new category</summary>
        /// <remarks>This api inserts a new category</remarks>
        /// <param name="category">Category to be inserted</param>
        /// <response code="200">Category inserted successfully</response>
        /// <response code="400">Process ran into an exception</response>  
        [HttpPost]
        [SwaggerRequestExample(typeof(Category), typeof(InsertCategory))]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> Post([FromBody]Category category)
        {
            try
            {
                string objectName = category.ProductFor + "-" + category.ProductType + ".jpg";
                //product.MinioObject_URL = WH.GetMinioObject("products", objectName).Result;
                //product.MinioObject_URL = AH.GetAmazonS3Object("product-category", objectName);
                category.MinioObject_URL = AH.GetS3Object("product-category", objectName);
                var collection = _db.GetCollection<Category>("Category");
                await collection.InsertOneAsync(category);
                return Ok(new ResponseData
                {
                    Code = "200",
                    Message = "Inserted",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("CategoryController", "Post", "Post", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Delete a category</summary>
        /// <param name="productFor">Whom is the product for</param>
        /// <param name="productType">Type of product</param>
        /// <remarks>This api deletes a category</remarks>
        /// <response code="200">Category deleted</response>
        /// <response code="404">Category not found</response> 
        /// <response code="400">Process ran into an exception</response>  
        [HttpDelete("{productFor}/{productType}")]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public ActionResult Delete(string productFor, string productType)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("ProductFor", productFor) & Builders<BsonDocument>.Filter.Eq("ProductType", productType);
                var product = MH.GetSingleObject(filter, "ProductDB", "Category").Result;
                if (product != null)
                {
                    var response = MH.DeleteSingleObject(filter, "ProductDB", "Category");
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Message = "Deleted",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "404",
                        Message = "Category Not Found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("CategoryController", "Delete", "Delete", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Update category details</summary>
        /// <param name="data">Details of category</param>
        /// <param name="productFor">For whom is the category</param>
        /// <param name="productType">Type of category</param>
        /// <response code="200">Category updated successfully</response>
        /// <response code="404">No category found</response>   
        /// <response code="400">Process ran into an exception</response>   
        [HttpPut("{productFor}/{productType}")]
        [SwaggerRequestExample(typeof(Category), typeof(UpdateCategory))]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> UpdateCategory([FromBody]Category data, string productFor, string productType)
        {
            try
            {
                var checkData = MH.CheckForDatas("ProductFor", productFor, "ProductType", productType, "ProductDB", "Category");
                if (checkData != null)
                {
                    var objectId = BsonSerializer.Deserialize<Category>(checkData).Id;
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
                    if(data.ProductFor!= null)
                    {
                        var value = BsonSerializer.Deserialize<Category>(MH.CheckForDatas("_id", objectId, null, null, "ProductDB", "Category")).ProductType;
                        var objectName = data.ProductFor + "-" + value;
                        GH.UpdateCategoryDetails(BsonSerializer.Deserialize<Category>(checkData).Id, productFor,productType, data.ProductFor, "ProductFor", objectName);
                    }
                    if(data.ProductType != null)
                    {
                        var value = BsonSerializer.Deserialize<Category>(MH.CheckForDatas("_id", objectId, null, null, "ProductDB", "Category")).ProductFor;
                        var objectName = value + "-" + data.ProductType;
                        GH.UpdateCategoryDetails(BsonSerializer.Deserialize<Category>(checkData).Id, productFor, productType, data.ProductType, "ProductType", objectName);
                    }
                    if(data.Description != null)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Category", Builders<BsonDocument>.Update.Set("Description", data.Description));
                    }
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Message = "Updated",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "404",
                        Message = "Category not found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("CategoryController", "UpdateCategory", "UpdateCategory", ex.Message);
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
