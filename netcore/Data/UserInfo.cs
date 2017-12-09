using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Arthur_Clive.Data
{
    /// <summary>Contains username of user </summary>
    public class Subscribe
    {
        /// <summary>ObjectId given by MongoDB</summary>
        public ObjectId Id { get; set; }
        /// <summary>UserName of user</summary>
        [Required]
        public string UserName { get; set; }
    }

    /// <summary>Get address list</summary>
    public class Datas
    {
        /// <summary>List Of Address Details</summary>
        public static List<Address> AddressList()
        {
            List<Address> addressList = new List<Address>();
            return addressList;
        }
    }

    /// <summary>Contains list of info about user</summary>
    public class AddressList
    {
        /// <summary>Address details of user</summary>
        [Required]
        public List<Address> ListOfAddress { get; set; }
    }

    /// <summary>Contains address details of user</summary>
    public class Address
    {
        /// <summary>ObjectId given by MongoDB</summary>
        public ObjectId Id { get; set; }
        /// <summary>UserName of user</summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>FullName of user</summary>
        [Required]
        public string Name { get; set; }
        /// <summary>PhoneNumber of user</summary>
        [Required]
        public string PhoneNumber { get; set; }
        /// <summary>Address Lines of user</summary>
        [Required]
        public string AddressLines { get; set; }
        /// <summary>Post office under which the address of user comes</summary>
        [Required]
        public string PostOffice { get; set; }
        /// <summary>City in which the user is located</summary>
        [Required]
        public string City { get; set; }
        /// <summary>State in which the user is located</summary>
        [Required]
        public string State { get; set; }
        /// <summary>Pincode under which the user address comes</summary>
        [Required]
        public string PinCode { get; set; }
        /// <summary>Lanmark for the address given by the user</summary>
        [Required]
        public string Landmark { get; set; }
        /// <summary>Billing address of the user</summary>
        [Required]
        public bool BillingAddress { get; set; }
        /// <summary>Shipping address of the user</summary>
        [Required]
        public bool ShippingAddress { get; set; }
        /// <summary>Flag to note that this is the default address</summary>
        [Required]
        public bool DefaultAddress { get; set; }
    }

    /// <summary>Contails list of products in cart</summary>
    public class CartList
    {
        /// <summary>List of cart products</summary>
        [Required]
        public List<Cart> ListOfProducts { get; set; }
    }

    /// <summary>Contains list of products in wishlist</summary>
    public class WishlistList
    {
        /// <summary>List of wishlist products</summary>
        [Required]
        public List<WishList> ListOfProducts { get; set; }
    }

    /// <summary>Contains details of product in cart</summary>
    public class Cart
    {
        /// <summary>ObjectId given by MongoDB</summary>
        public ObjectId Id { get; set; }
        /// <summary>UserName of the user</summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>SKU of the product</summary>
        [Required]
        public string ProductSKU { get; set; }
        /// <summary>Url of the image given to describe the product</summary>
        [Required]
        public string MinioObject_URL { get; set; }
        /// <summary>For whom is the product</summary>
        [Required]
        public string ProductFor { get; set; }
        /// <summary>Type of the product</summary>
        [Required]
        public string ProductType { get; set; }
        /// <summary>Design on the product </summary>
        [Required]
        public string ProductDesign { get; set; }
        /// <summary>Brand of the product</summary>
        [Required]
        public string ProductBrand { get; set; }
        /// <summary>Price for the product</summary>
        [Required]
        public double ProductPrice { get; set; }
        /// <summary>Discount percentage fot the product</summary>
        [Required]
        public double ProductDiscount { get; set; }
        /// <summary>Discount price for the product</summary>
        [Required]
        public double ProductDiscountPrice { get; set; }
        /// <summary>Quantity of the product ordered</summary>
        [Required]
        public long ProductQuantity { get; set; }
        /// <summary>Size of the product</summary>
        [Required]
        public string ProductSize { get; set; }
        /// <summary>Colour of the product</summary>
        [Required]
        public string ProductColour { get; set; }
        /// <summary>Description about the product</summary>
        [Required]
        public string ProductDescription { get; set; }
    }

    /// <summary>Contains details of product in wishlist</summary>
    public class WishList
    {
        /// <summary>ObjectId given by MongoDB</summary>
        public ObjectId Id { get; set; }
        /// <summary>UserName of the user</summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>SKU of the product</summary>
        [Required]
        public string ProductSKU { get; set; }
        /// <summary>Url of the image given to describe the product</summary>
        [Required]
        public string MinioObject_URL { get; set; }
        /// <summary>For whom is the product</summary>
        [Required]
        public string ProductFor { get; set; }
        /// <summary>Type of the product</summary>
        [Required]
        public string ProductType { get; set; }
        /// <summary>Design on the product </summary>
        [Required]
        public string ProductDesign { get; set; }
        /// <summary>Brand of the product</summary>
        [Required]
        public string ProductBrand { get; set; }
        /// <summary>Price for the product</summary>
        [Required]
        public double ProductPrice { get; set; }
        /// <summary>Discount percentage fot the product</summary>
        [Required]
        public double ProductDiscount { get; set; }
        /// <summary>Discount price for the product</summary>
        [Required]
        public double ProductDiscountPrice { get; set; }
        /// <summary>Size of the product</summary>
        [Required]
        public string ProductSize { get; set; }
        /// <summary>Colour of the product</summary>
        [Required]
        public string ProductColour { get; set; }
        /// <summary>Description about the product</summary>
        [Required]
        public string ProductDescription { get; set; }
    }
}
