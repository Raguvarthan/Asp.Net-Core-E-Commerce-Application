using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Arthur_Clive.Logger
{
    /// <summary>Data to be inserted to server side log</summary>
    public class ApplicationLogger
    {
        /// <summary>ObjectId given by MongoDB</summary>
        public ObjectId Id { get; set; }
        /// <summary>Name of controller where error occurs</summary>
        [BsonElement("Controller")]
        public string Controller { get; set; }
        /// <summary>Method where error occurs</summary>
        [BsonElement("Method")]
        public string Method { get; set; }
        /// <summary>Method name where error occurs</summary>
        [BsonElement("MethodName")]
        public string MethodName { get; set; }
        /// <summary>Description of error</summary>
        [BsonElement("Description")]
        public string Description { get; set; }
    }
}
