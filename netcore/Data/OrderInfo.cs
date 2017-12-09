using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Arthur_Clive.Data
{
    /// <summary>Contains the info of the order made</summary>
    public class OrderInfo
    {
        /// <summary>ObjectId give by MongoDB</summary>
        public ObjectId Id { get; set; }
        /// <summary>Id given to the placed order</summary>
        [Required]
        public int OrderId { get; set; }
        /// <summary>Email or PhoneNumber of the user</summary>
        public string UserName { get; set; }
        /// <summary>Payment method prefered by the user</summary>
        [Required]
        public string PaymentMethod { get; set; }
        /// <summary>Total amount to be paid for order</summary>
        public double TotalAmount { get; set; }
        /// <summary>Discounted amount</summary>
        public double CouponDiscount { get; set; }
        /// <summary>Payment method and status of the payment</summary>
        public PaymentMethod PaymentDetails { get; set; }
        /// <summary>Address details of user</summary>
        public List<Address> Address { get; set; }
        /// <summary>Product details of the order</summary>
        public List<ProductDetails> ProductDetails { get; set; }
    }

    /// <summary>Contails the details related to ordered product</summary>
    public class ProductDetails
    {
        /// <summary>SKU given to the product</summary>
        public string ProductSKU { get; set; }
        /// <summary>Delivery status of the product</summary>
        public string Status { get; set; }
        /// <summary>Code given to the delivery status</summary>
        public List<StatusCode> StatusCode { get; set; }
        /// <summary>Product details</summary>
        public Cart ProductInCart { get; set; }
    }

    /// <summary>Contails the status details</summary>
    public class StatusCode
    {
        /// <summary>Id given to the status</summary>
        public int StatusId { get; set; }
        /// <summary>Description of the status</summary>
        public string Description { get; set; }
        /// <summary>Date and time when the status is registered</summary>
        public DateTime Date { get; set; }
    }

    /// <summary>Contails payment details for the product</summary>
    public class PaymentMethod
    {
        /// <summary>Method of the payment</summary>
        public string Method { get; set; }
        /// <summary>Status of the payment</summary>
        public List<StatusCode> Status { get; set; }
    }
    
    /// <summary>Contails update details for the order</summary>
    public class StatusUpdate
    {
        /// <summary>Id of the order for which the status is to be updated</summary>
        [Required]
        public int OrderId { get; set; }
        /// <summary>Status to be updated</summary>
        [Required]
        public string Status { get; set; }
    }
}
