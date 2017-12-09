using MongoDB.Driver;
using WH = AuthorizedServer.Helper.MongoHelper;

namespace AuthorizedServer.Logger
{
    /// <summary>Data access for logger</summary>
    public class LoggerDataAccess
    {
        /// <summary>Get Mongo Database</summary>
        public static IMongoDatabase _db = WH._client.GetDatabase("ArthurCliveLogDB");

        /// <summary>Create server side log</summary>
        /// <param name="controllerName"></param>
        /// <param name="methodName"></param>
        /// <param name="method"></param>
        /// <param name="errorDescription"></param>
        public static void CreateLog(string controllerName, string methodName, string method, string errorDescription)
        {
            ApplicationLogger logger =
                new ApplicationLogger
                {
                    Controller = controllerName,
                    MethodName = methodName,
                    Method = method,
                    Description = errorDescription
                };
            var collection = _db.GetCollection<ApplicationLogger>("ServerLog");
            collection.InsertOneAsync(logger);
        }
    }
}
