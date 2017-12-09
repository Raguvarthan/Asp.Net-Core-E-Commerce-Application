using MongoDB.Driver;
using MH = Arthur_Clive.Helper.MongoHelper;

namespace Arthur_Clive.Logger
{
    /// <summary>Data access for server side logger</summary>
    public class LoggerDataAccess
    {
        /// <summary>Mongo Database</summary>
        public static IMongoDatabase _db = MH._client.GetDatabase("ArthurCliveLogDB");

        /// <summary>Create log for server side error</summary>
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
