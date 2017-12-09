using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Minio;
using System;
using Arthur_Clive.Data;
using Arthur_Clive.Logger;
using System.Threading.Tasks;
using MongoDB.Driver;
using AH = Arthur_Clive.Helper.AmazonHelper;
using WH = Arthur_Clive.Helper.MinioHelper;
using MH = Arthur_Clive.Helper.MongoHelper;
using GH = Arthur_Clive.Helper.GlobalHelper;
using Swashbuckle.AspNetCore.Examples;
using Arthur_Clive.Swagger;
using MongoDB.Bson.Serialization;

namespace Arthur_Clive.Controllers
{
    /// <summary>Controller to get, post and delete products</summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        /// <summary></summary>
        public IMongoDatabase _db = MH._client.GetDatabase("ProductDB");

        /// <summary>Get all the products </summary>
        /// <remarks>This api is used to get all the products</remarks>
        /// <response code="200">Returns products</response>
        /// <response code="404">No products found</response> 
        /// <response code="400">Process ran into an exception</response>   
        [HttpGet]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> Get()
        {
            try
            {
                var collection = _db.GetCollection<Product>("Product");
                var filter = FilterDefinition<Product>.Empty;
                IAsyncCursor<Product> cursor = await collection.FindAsync(filter);
                var products = cursor.ToList();
                if (products.Count > 0)
                {
                    foreach (var data in products)
                    {
                        string objectName = data.ProductSKU + ".jpg";
                        //data.ObjectURL = WH.GetMinioObject("products", objectName).Result;
                        //data.ObjectURL = AH.GetAmazonS3Object("arthurclive-products", objectName);
                        data.MinioObject_URL = AH.GetS3Object("arthurclive-products", objectName);
                    }
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Message = "Success",
                        Data = products
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "404",
                        Message = "No products found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("ProductController", "Get", "Get", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Get products with filter</summary>
        /// <param name="productFor">For whom is the product</param>
        /// <param name="productType">type of product</param>
        /// <param name="productDesign">Design on product</param>
        /// <response code="200">Returns products that match the filters</response>
        /// <response code="404">No products found</response> 
        /// <response code="400">Process ran into an exception</response> 
        [HttpGet("{productFor}/{productType}/{productDesign}")]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> GetProductByFilter(string productFor, string productType, string productDesign)
        {
            try
            {
                var collection = _db.GetCollection<Product>("Product");
                var filter = Builders<Product>.Filter.Eq("ProductFor", productFor) & Builders<Product>.Filter.Eq("ProductType", productType) & Builders<Product>.Filter.Eq("ProductDesign", productDesign);
                IAsyncCursor<Product> cursor = await collection.FindAsync(filter);
                var products = cursor.ToList();
                if (products.Count > 0)
                {
                    foreach (var data in products)
                    {
                        string objectName = data.ProductSKU + ".jpg";
                        //data.ObjectURL = WH.GetMinioObject("products", objectName).Result;
                        //data.ObjectURL = AH.GetAmazonS3Object("arthurclive-products", objectName);
                        data.MinioObject_URL = AH.GetS3Object("arthurclive-products", objectName);
                    }
                    return Ok(new ResponseData
                    {
                        Code = "200",
                        Message = "Success",
                        Data = products
                    });
                }
                else
                {
                    return BadRequest(new ResponseData
                    {
                        Code = "404",
                        Message = "No products found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("ProductController", "Get", "Get", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Insert a new products </summary>
        /// <remarks>This api is used to insert a new product</remarks>
        /// <param name="product">Details of product to be inserted</param>
        /// <response code="200">Product inserted successfully</response>
        /// <response code="400">Process ran into an exception</response>   
        [HttpPost]
        [SwaggerRequestExample(typeof(Product), typeof(InsertProduct))]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> Post([FromBody]Product product)
        {
            try
            {
                product.ProductDiscountPrice = (product.ProductPrice - (product.ProductPrice * (product.ProductDiscount / 100)));
                product.ProductSKU = product.ProductFor + "-" + product.ProductType + "-" + product.ProductDesign + "-" + product.ProductColour + "-" + product.ProductSize;
                string objectName = product.ProductSKU + ".jpg";
                //product.MinioObject_URL = WH.GetMinioObject("arthurclive-products", objectName).Result;
                //product.MinioObject_URL = AH.GetAmazonS3Object("arthurclive-products", objectName);
                product.MinioObject_URL = AH.GetS3Object("arthurclive-products", objectName);
                var collection = _db.GetCollection<Product>("Product");
                await collection.InsertOneAsync(product);
                return Ok(new ResponseData
                {
                    Code = "200",
                    Message = "Inserted",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("ProductController", "Post", "Post", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Delete a product</summary>
        /// <param name="productSKU">SKU of product to be deleted</param>
        /// <remarks>This api is used to delete a product</remarks>
        /// <response code="200">Product deleted successfully</response>
        /// <response code="404">No product found</response>   
        /// <response code="400">Process ran into an exception</response>   
        [HttpDelete("{productSKU}")]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public ActionResult Delete(string productSKU)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Product_SKU", productSKU);
                var product = MH.GetSingleObject(filter, "ProductDB", "Product").Result;
                if (product != null)
                {
                    var authCollection = _db.GetCollection<Product>("Product");
                    var response = authCollection.DeleteOneAsync(product);
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
                        Message = "Product Not Found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("ProductController", "Delete", "Delete", ex.Message);
                return BadRequest(new ResponseData
                {
                    Code = "400",
                    Message = "Failed",
                    Data = ex.Message
                });
            }
        }

        /// <summary>Update product details</summary>
        /// <param name="data">Details of product</param>
        /// <param name="productSKU">SKU of product to be updated</param>
        /// <response code="200">Product updated successfully</response>
        /// <response code="404">No product found</response>   
        /// <response code="400">Process ran into an exception</response>   
        [HttpPut("{productSKU}")]
        [SwaggerRequestExample(typeof(Product), typeof(UpdateProduct))]
        [ProducesResponseType(typeof(ResponseData), 200)]
        public async Task<ActionResult> UpdateProduct([FromBody]Product data,string productSKU)
        {
            try
            {
                var checkData = MH.CheckForDatas("ProductSKU",productSKU,null,null,"ProductDB","Product");
                if (checkData != null)
                {
                    var objectId = BsonSerializer.Deserialize<Product>(checkData).Id;
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
                    if (data.ProductFor != null)
                    {
                        productSKU = BsonSerializer.Deserialize<Product>(MH.CheckForDatas("_id", objectId, null, null, "ProductDB", "Product")).ProductSKU;
                        var objectName = data.ProductFor + "-" + productSKU.Split('-')[1] + "-" + productSKU.Split('-')[2] + "-" + productSKU.Split('-')[3] + "-" + productSKU.Split('-')[4];
                        GH.UpdateProductDetails(BsonSerializer.Deserialize<Product>(checkData).Id, productSKU,data.ProductFor, "ProductFor",objectName);
                    }
                    if (data.ProductType != null)
                    {
                        productSKU = BsonSerializer.Deserialize<Product>(MH.CheckForDatas("_id", objectId, null, null, "ProductDB", "Product")).ProductSKU;
                        var objectName = productSKU.Split('-')[0] + "-" + data.ProductType + "-" + productSKU.Split('-')[2] + "-" + productSKU.Split('-')[3] + "-" + productSKU.Split('-')[4];
                        GH.UpdateProductDetails(BsonSerializer.Deserialize<Product>(checkData).Id, productSKU, data.ProductType, "ProductType",objectName);
                    }
                    if (data.ProductDesign != null)
                    {
                        productSKU = BsonSerializer.Deserialize<Product>(MH.CheckForDatas("_id", objectId, null, null, "ProductDB", "Product")).ProductSKU;
                        var objectName = productSKU.Split('-')[0] + "-" + productSKU.Split('-')[1] + "-" + data.ProductDesign + "-" + productSKU.Split('-')[3] + "-" + productSKU.Split('-')[4];
                        GH.UpdateProductDetails(BsonSerializer.Deserialize<Product>(checkData).Id, productSKU, data.ProductDesign, "ProductDesign",objectName);
                    }
                    if (data.ProductBrand != null)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductBrand", data.ProductBrand));
                    }
                    if (data.ProductPrice > 0)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductPrice", data.ProductPrice));
                        double discountPercentage;
                        if(data.ProductDiscount > 0)
                        {
                            discountPercentage = data.ProductDiscount;
                        }
                        else
                        {
                            discountPercentage = BsonSerializer.Deserialize<Product>(MH.CheckForDatas("ProductSKU", productSKU, null, null, "ProductDB", "Product")).ProductDiscount;
                        }
                        var discountPrice = (data.ProductPrice - (data.ProductPrice * (discountPercentage / 100)));


                        var update1 = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductDiscountPrice", discountPrice));
                    }
                    if (data.ProductDiscount > 0)
                    {

                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductDiscount", data.ProductDiscount));
                        double price;
                        if (data.ProductPrice > 0)
                        {
                            price = data.ProductPrice;
                        }
                        else
                        {
                            price = BsonSerializer.Deserialize<Product>(MH.CheckForDatas("ProductSKU", productSKU, null, null, "ProductDB", "Product")).ProductPrice;
                        }
                        var discountPrice = (price - (price * (data.ProductDiscount / 100)));
                        var update1 = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductDiscountPrice", discountPrice));
                    }
                    if (data.ProductStock > 0)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductStock", data.ProductStock));
                    }
                    if (data.ProductSize != null)
                    {
                        productSKU = BsonSerializer.Deserialize<Product>(MH.CheckForDatas("_id", objectId, null, null, "ProductDB", "Product")).ProductSKU;
                        var objectName = productSKU.Split('-')[0] + "-" + productSKU.Split('-')[1] + "-" + productSKU.Split('-')[2] + "-" + productSKU.Split('-')[3] + "-" + data.ProductSize;
                        GH.UpdateProductDetails(BsonSerializer.Deserialize<Product>(checkData).Id, productSKU, data.ProductSize, "ProductSize",objectName);
                    }
                    if (data.ProductMaterial != null)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductMaterial", data.ProductMaterial));
                    }
                    if (data.ProductRating > 0)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductRating", data.ProductRating));
                    }
                    if (data.ProductReviews != null)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductReviews", data.ProductReviews));
                    }
                    if (data.ProductColour != null)
                    {
                        productSKU = BsonSerializer.Deserialize<Product>(MH.CheckForDatas("_id", objectId, null, null, "ProductDB", "Product")).ProductSKU;
                        var objectName = productSKU.Split('-')[0] + "-" + productSKU.Split('-')[1] + "-" + productSKU.Split('-')[2] + "-" + data.ProductColour + "-" + productSKU.Split('-')[4];
                        GH.UpdateProductDetails(BsonSerializer.Deserialize<Product>(checkData).Id, productSKU, data.ProductColour, "ProductColour",objectName);
                    }
                    if (data.RefundApplicable != null)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("RefundApplicable", data.RefundApplicable));
                    }
                    if (data.ReplacementApplicable != null)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ReplacementApplicable", data.ReplacementApplicable));
                    }
                    if (data.ProductDescription != null)
                    {
                        var update = await MH.UpdateSingleObject(filter, "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductDescription", data.ProductDescription));
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
                        Message = "Product not found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("ProductController", "UpdateProduct", "UpdateProduct", ex.Message);
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
