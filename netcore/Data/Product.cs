using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Arthur_Clive.Data
{
    /// <summary>Contains details of category</summary>
    public class Category
    {
        /// <summary>ObjectId give by MongoDB</summary>
        public ObjectId Id { get; set; }
        /// <summary>For whom is the category</summary>
        [Required]
        public string ProductFor { get; set ; }
        /// <summary>Type of the category</summary>
        [Required]
        public string ProductType { get; set; }
        /// <summary>Url for the image added to describe the category</summary>
        public string MinioObject_URL { get; set; }
        /// <summary>Description for the added categoty</summary>
        [Required]
        public string Description { get; set; }
    }

    /// <summary>Contains details of product</summary>
    public class Product
    {
        /// <summary>ObjectId give by MongoDB</summary>
        public ObjectId Id { get; set; }
        /// <summary>SKU for the product</summary>
        [Required]
        public string ProductSKU { get; set; }
        /// <summary>Url of the image added to describe the product</summary>
        public string MinioObject_URL { get; set; }
        /// <summary>For whom is the product</summary>
        [Required]
        public string ProductFor { get; set; }
        /// <summary>Type of the product</summary>
        [Required]
        public string ProductType { get; set; }
        /// <summary>Design on the product</summary>
        [Required]
        public string ProductDesign { get; set; }
        /// <summary>Brand of the product</summary>
        [Required]
        public string ProductBrand { get; set; }
        /// <summary>Price of the product</summary>
        [Required]
        public double ProductPrice { get; set; }
        /// <summary>Percentage of discount offered for the product</summary>
        [Required]
        public double ProductDiscount { get; set; }
        /// <summary>Discount price for the product</summary>
        public double ProductDiscountPrice { get; set; }
        /// <summary>Stock details of the product</summary>
        [Required]
        public long ProductStock { get; set; }
        /// <summary>Size of the product</summary>
        [Required]
        public string ProductSize { get; set; }
        /// <summary>Material of the product</summary>
        [Required]
        public string ProductMaterial { get; set; }
        /// <summary>Rating given to the product by the users</summary>
        public double ProductRating { get; set; }
        /// <summary>Reviews given to the product by the users</summary>
        public Review[] ProductReviews { get; set; }
        /// <summary>Colour of the product</summary>
        [Required]
        public string ProductColour { get; set; }
        /// <summary>Refund applicable details for the product</summary>
        [Required]
        public bool? RefundApplicable { get; set; }
        /// <summary>Replacement applicable details for the product</summary>
        [Required]
        public bool? ReplacementApplicable { get; set; }
        /// <summary>Description for the product</summary>
        [Required]
        public string ProductDescription { get; set; }
    }

    /// <summary>Details of review added by user</summary>
    public class Review
    {
        /// <summary>Name of the user who adds the review</summary>
        [Required]
        public string Name { get; set; }
        /// <summary>Review given by the user</summary>
        [Required]
        public string Comment { get; set; }
    }
}
