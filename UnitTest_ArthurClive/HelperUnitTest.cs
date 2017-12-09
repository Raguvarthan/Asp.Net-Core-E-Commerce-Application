using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minio;
using MongoDB.Driver;
using AH = Arthur_Clive.Helper.AmazonHelper;
using MOH = Arthur_Clive.Helper.MongoHelper;
using MH = Arthur_Clive.Helper.MinioHelper;
using MongoDB.Bson;
using Arthur_Clive.Data;
using Arthur_Clive.Helper;

namespace UnitTest_ArthurClive
{
    [TestClass]
    public class HelperUnitTest
    {
        public HelperUnitTest()
        {
            Connect();
            Clean();
            InsertData_For_Category();
            InsertData_For_Product();
        }

        [TestMethod]
        public void Helper_Amazon_GetS3ObjectUrl()
        {
            //Arrange
            string bucketName = "sampleBucketName";
            string objectName = "sampleObjectName";
            //Act
            var result = AH.GetS3Object(bucketName, objectName) as string;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(AH.s3PrefixUrl + "sampleBucketName/sampleObjectName", result);
        }

        [TestMethod]
        public void Helper_Amazon_GetAmazonS3ObjectPresignedUrl()
        {
            //Arrange
            string bucketName = "sampleBucketName";
            string objectName = "sampleObjectName";
            string expectedUrl = "https://s3.ap-south-1.amazonaws.com/sampleBucketName/sampleObjectName?X-Amz-Expires=300&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAIUAYVIL7A7I6XECA/ap-south-1/s3/aws4_request&X-Amz-Date=";
            //Act
            var result = AH.GetAmazonS3Object(bucketName, objectName) as string;
            //Get the substring from the result removing the date  and amazon signed signatures as it varies every time
            var subString = result.Split('=')[0] + "=" + result.Split('=')[1] + "=" + result.Split('=')[2] + "=" + (result.Split('=')[3]).Split('/')[0] + "/" + (result.Split('=')[3]).Split('/')[2] + "/" + (result.Split('=')[3]).Split('/')[3] + "/" + result.Split('=')[3].Substring(result.Split('=')[3].LastIndexOf('/') + 1) + "=";
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUrl, subString);

        }

        [TestMethod]
        public void Helper_Amazon_GetAmazonS3Client()
        {
            //Arrange

            //Act
            var result = AH.GetAmazonS3Client() as IAmazonS3;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Helper_Minio_GetMinioClient()
        {
            //Arrange

            //Act
            var result = MH.GetMinioClient() as MinioClient;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Helper_Minio_GetMinioObjectPresignedUrl()
        {
            //Arrange
            string bucketName = "product-category";
            string objectName = "All-Art.jpg";
            string expectedUrl = "http://localhost:9000/product-category/All-Art.jpg?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=MinioServer%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=";
            //Act
            var result = MH.GetMinioObject(bucketName, objectName) as Task<string>;
            //Get the substring from the result removing the date  and amazon signed signatures as it varies every time
            var subString = result.Result.Split('=')[0] + "=" + result.Result.Split('=')[1] + "=" + (result.Result.Split('=')[2]).Split('%')[0] + "%" + (result.Result.Split('=')[2]).Split('%')[2] + "%" + (result.Result.Split('=')[2]).Split('%')[3] + "%" + result.Result.Split('=')[2].Substring(result.Result.Split('=')[2].LastIndexOf('%') + 1) + "=";
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUrl, subString);
        }

        [TestMethod]
        public void Helper_Mongo_GetMongoClient()
        {
            //Arrange

            //Act
            var result = MOH.GetClient() as MongoClient;
            //Assert
            Assert.IsNotNull(result);
        }

        private MongoServer server;
        private MongoDatabase productDatabase;

        private void Connect()
        {
            server = new MongoServer(new MongoServerSettings());
            productDatabase = server.GetDatabase("UnitTestDB");
        }

        private void Clean()
        {
            var categoryCollection = productDatabase.GetCollection<Category>("category");
            categoryCollection.RemoveAll();
            var productCollection = productDatabase.GetCollection<Product>("product");
            productCollection.RemoveAll();
        }

        private void InsertData_For_Category()
        {
            var category1 = new Category { ProductFor = "Men", ProductType = "Shirts"};
            var category2 = new Category { ProductFor = "Women", ProductType = "Saree"};
            var category3 = new Category { ProductFor = "Boys", ProductType = "Shirts"};
            var category4 = new Category { ProductFor = "Girls", ProductType = "Tops"};
            var category5 = new Category { ProductFor = "All", ProductType = "Art" };
            
            var productCollection = productDatabase.GetCollection<Category>("category");
            productCollection.Insert(category1);
            productCollection.Insert(category2);
            productCollection.Insert(category3);
            productCollection.Insert(category4);
            productCollection.Insert(category5);
        }
        
        private void InsertData_For_Product()
        {
            var product1 = new Product { ProductFor = "Men", ProductType = "Shirts", ProductColour = "Blue",ProductSize="S" };
            var product2 = new Product { ProductFor = "Men", ProductType = "Shirts", ProductColour = "Black", ProductSize = "S" };
            var product3 = new Product { ProductFor = "Men", ProductType = "Shirts", ProductColour = "White", ProductSize = "S" };
            var product4 = new Product { ProductFor = "Men", ProductType = "Shirts", ProductColour = "Blue", ProductSize = "M" };
            
            var productCollection = productDatabase.GetCollection<Product>("product");
            productCollection.Insert(product1);
            productCollection.Insert(product2);
            productCollection.Insert(product3);
            productCollection.Insert(product4);
        }

        [TestMethod]
        public void Helper_Mongo_GetAllCategories()
        {
            //Arrange
            var collectionName = "category";
            //Act
            var result = productDatabase.GetCollection<Category>(collectionName).Count();
            //Assert
            Assert.AreNotEqual(result, 0);
            Assert.AreEqual(result, 4);
        }
        
        [TestMethod]
        public void Helper_Mongo_GetSingleObjectInGB()
        {
            //Arrange
            var dbName = "UnitTestDB";
            var collectionName = "Product";
            var productFor = "Mens";
            var productType = "Shirts";
            MongoHelper helper = new MongoHelper();
            //Act
            var filter = Builders<BsonDocument>.Filter.Eq("ProductFor", productFor) & Builders<BsonDocument>.Filter.Eq("ProductType", productType);
            var result = MOH.GetSingleObject(filter,dbName,collectionName);
            //Assert
            Assert.IsNotNull(result.Result);
            //Assert.AreEqual(result.Result,);
        }

        //public AuthHelper authHelper = new AuthHelper();
        //private IOptions<Audience> _settings;
        //private IRTokenRepository _repo;

        //[TestInitialize]
        //public void Initialize(IOptions<Audience> settings, IRTokenRepository repo)
        //{
        //    this._settings = settings;
        //    this._repo = repo;
        //}

        //[TestMethod]
        //public void Helper_AuthHelper_GetJWTAccessToken()
        //{
        //    //Arrange
        //    Parameters parameters = new Parameters();
        //    parameters.username = "sampleUser";
        //    string code = "999";
        //    string message = "OK";
        //    //Act
        //    var result = authHelper.DoPassword(parameters, _repo, _settings) as ResponseData;
        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(result.Code, code);
        //    Assert.AreEqual(result.Message, message);
        //    Assert.IsNotNull(result.Data);
        //}
    }

}
