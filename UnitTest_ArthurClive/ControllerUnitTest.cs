using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Arthur_Clive.Data;
using Arthur_Clive.Helper;
using Arthur_Clive.Logger;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using AH = Arthur_Clive.Helper.AmazonHelper;
using WH = Arthur_Clive.Helper.MinioHelper;
using MH = Arthur_Clive.Helper.MongoHelper;
using MongoDB.Bson;
using Arthur_Clive.Controllers;
using System.Collections.Generic;

namespace UnitTest_ArthurClive
{
    [TestClass]
    public class ControllerUnitTest
    {
        [TestMethod]
        public void Controller_Category_GetMethod()
        {
            //Arrange
            CategoryController controller = new CategoryController();
            //Act
            var result = controller.Get() as Task<ActionResult>;
            var viewResult = result.Result.ToBsonDocument();
            var filter = viewResult["Value"].AsBsonDocument;
            var code = filter["Code"].AsString;
            var data = filter["Data"].AsBsonDocument;
            var message = filter["Message"].AsString;
            //Assert
            Assert.IsNotNull(result.Result);
            Assert.AreEqual("200",code);
            Assert.AreEqual("Success",message);
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void Controller_Category_PostMethod()
        {
            //Arrange
            CategoryController controller = new CategoryController();
            //Act
            var result = controller.Post(new Category()) as Task<ActionResult>;
            var viewResult = result.Result.ToBsonDocument();
            var filter = viewResult["Value"].AsBsonDocument;
            var code = filter["Code"].AsString;
            var message = filter["Message"].AsString;
            //Assert
            Assert.IsNotNull(result.Result);
            Assert.AreEqual("200", code);
            Assert.AreEqual("Inserted",message);
        }

        [TestMethod]
        public void Controller_Category_DeleteMethod()
        {
            //Arrange
            CategoryController controller = new CategoryController();
            //Act
            var result = controller.Delete(null,null) as ActionResult;
            var viewResult = result.ToBsonDocument();
            var filter = viewResult["Value"].AsBsonDocument;
            var code = filter["Code"].AsString;
            var message = filter["Message"].AsString;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("200", code);
            Assert.AreEqual("Deleted", message);
        }

        [TestMethod]
        public void Controller_SubCategory_GetMethod()
        {
            //Arrange
            SubCategoryController controller = new SubCategoryController();
            var productFor = "All";
            var productType = "Art";
            //Act
            var result = controller.Get(productFor,productType).Result as ActionResult;
            var viewResult = result.ToBsonDocument();
            var filter = viewResult["Value"].AsBsonDocument;
            var code = filter["Code"].AsString;
            var message = filter["Message"].AsString;
            var data = filter["Data"].AsBsonDocument;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("200", code);
            Assert.AreEqual("Success", message);
            Assert.IsNotNull(data);
        }
    }
}
