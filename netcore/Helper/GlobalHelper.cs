using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Arthur_Clive.Data;
using Arthur_Clive.Logger;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Minio;
using AH = Arthur_Clive.Helper.AmazonHelper;
using WH = Arthur_Clive.Helper.MinioHelper;
using MH = Arthur_Clive.Helper.MongoHelper;
using MongoDB.Bson;

namespace Arthur_Clive.Helper
{
    /// <summary>Global helper method</summary>
    public class GlobalHelper
    {
        /// <summary>xml file</summary>
        public static XElement ReadXML()
        {
            var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var xmlStr = File.ReadAllText(Path.Combine(dir, "AmazonKeys.xml"));
            return XElement.Parse(xmlStr);
        }

        /// <summary>Get order list from MongoDB</summary>
        /// <param name="username"></param>
        /// <param name="order_db"></param>
        public async static Task<List<OrderInfo>> GetOrders(string username, IMongoDatabase order_db)
        {
            try
            {
                IAsyncCursor<OrderInfo> cursor = await order_db.GetCollection<OrderInfo>("OrderInfo").FindAsync(Builders<OrderInfo>.Filter.Eq("UserName", username));
                var orders = cursor.ToList();
                return orders;
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("GlobalHelper", "GetOrders", "GetOrders", ex.Message);
                return null;
            }
        }

        /// <summary>Get product list  from MongoDB</summary>
        /// <param name="productSKU"></param>
        /// <param name="product_db"></param>
        public async static Task<List<Product>> GetProducts(string productSKU, IMongoDatabase product_db)
        {
            try
            {
                IAsyncCursor<Product> productCursor = await product_db.GetCollection<Product>("Product").FindAsync(Builders<Product>.Filter.Eq("ProductSKU", productSKU));
                var products = productCursor.ToList();
                return products;
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("GlobalHelper", "GetProducts", "GetProducts", ex.Message);
                return null;
            }
        }

        /// <summary></summary>
        /// <param name="objectId"></param>
        /// <param name="productSKU"></param>
        /// <param name="updateData"></param>
        /// <param name="updateField"></param>
        /// <param name="objectName"></param>
        public async static void UpdateProductDetails(dynamic objectId,string productSKU, dynamic updateData , string updateField,string objectName)
        {
            try
            {
                var update = await MH.UpdateSingleObject(Builders<BsonDocument>.Filter.Eq("_id", objectId), "ProductDB", "Product", Builders<BsonDocument>.Update.Set(updateField, updateData));
                string MinioObject_URL;
                //MinioObject_URL = WH.GetMinioObject("arthurclive-products", objectName).Result;
                //MinioObject_URL = AH.GetAmazonS3Object("arthurclive-products", objectName);
                MinioObject_URL = AH.GetS3Object("arthurclive-products", objectName);
                var update1 = await MH.UpdateSingleObject(Builders<BsonDocument>.Filter.Eq("_id", objectId), "ProductDB", "Product", Builders<BsonDocument>.Update.Set("ProductSKU", objectName));
                var update2 = await MH.UpdateSingleObject(Builders<BsonDocument>.Filter.Eq("_id", objectId), "ProductDB", "Product", Builders<BsonDocument>.Update.Set("MinioObject_URL", MinioObject_URL));
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("GlobalHelper", "UpdateProductDetails", "UpdateProductDetails", ex.Message);
            }
        }

        /// <summary></summary>
        /// <param name="objectId"></param>
        /// <param name="productFor"></param>
        /// <param name="productType"></param>
        /// <param name="updateData"></param>
        /// <param name="updateField"></param>
        /// <param name="objectName"></param>
        public async static void UpdateCategoryDetails(dynamic objectId, string productFor,string productType, dynamic updateData, string updateField, string objectName)
        {
            try
            {
                var update = await MH.UpdateSingleObject(Builders<BsonDocument>.Filter.Eq("_id", objectId), "ProductDB", "Category", Builders<BsonDocument>.Update.Set(updateField, updateData));
                string MinioObject_URL;
                //MinioObject_URL = WH.GetMinioObject("products", objectName).Result;
                //MinioObject_URL = AH.GetAmazonS3Object("product-category", objectName);
                MinioObject_URL = AH.GetS3Object("product-category", objectName);
                var update1 = await MH.UpdateSingleObject(Builders<BsonDocument>.Filter.Eq("_id", objectId), "ProductDB", "Category", Builders<BsonDocument>.Update.Set("MinioObject_URL", MinioObject_URL));
            }
            catch (Exception ex)
            {
                LoggerDataAccess.CreateLog("GlobalHelper", "UpdateCategoryDetails", "UpdateCategoryDetails", ex.Message);
            }
        }
    }
}
