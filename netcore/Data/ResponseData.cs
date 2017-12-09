
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Arthur_Clive.Data
{
    /// <summary>Contails responce data</summary>
    public class ResponseData
    {
        /// <summary>Responce code for the responce</summary>
        [Required]
        public string Code { get; set; }
        /// <summary>Message in the responce</summary>
        public string Message { get; set; }
        /// <summary>Data in the responce</summary>
        public object Data { get; set; }
        /// <summary>Other contents of responce</summary>
        public object Content { get; set; }
    }

    /// <summary>Details of coupon</summary>
    public class Coupon
    {
        /// <summary></summary>
        public ObjectId Id { get; set; } 
        /// <summary>Coupon code</summary>
        [Required]
        public string Code { get; set; }
        /// <summary>For whom is the coupon applicable for</summary>
        [Required]
        public string ApplicableFor { get; set; }
        /// <summary>Expiry time of the coupon</summary>
        [Required]
        public DateTime ExpiryTime { get; set; }
        /// <summary>Coupon usage count</summary>
        [Required]
        public int UsageCount { get; set; }
        /// <summary>Value of coupon in percentage or amount</summary>
        [Required]
        public double Value { get; set; }
        /// <summary>If the value of coupon is persentage pass the flag as true</summary>
        [Required]
        public bool? Percentage { get; set; }
    }
}
