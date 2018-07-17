namespace Chakra.Core.MongoDb.Data.Options
{
    /// <summary>
    /// Interface for store options for MongoDB
    /// </summary>
    public interface IMongoDbOptions
    {        
        /// <summary>
        /// Connection string for database
        /// </summary>
        string ConnectionString{ get; set; }
    }
}
