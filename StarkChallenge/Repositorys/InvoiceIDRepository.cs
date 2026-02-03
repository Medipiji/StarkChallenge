using MongoDB.Bson;
using MongoDB.Driver;
using StarkChallenge.Interfaces.IRepositorys;

namespace StarkChallenge.Repositorys
{
    public class InvoiceIDRepository : IInvoiceIDRepository
    {
        private readonly IConfiguration _configuration;
        public InvoiceIDRepository(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        protected (bool, IMongoCollection<BsonDocument>) ConnectToDataBase()
        {
            try
            {
                var client = new MongoClient(_configuration.GetConnectionString("MongoDb"));
                var database = client.GetDatabase("sample_mflix");
                var collection = database.GetCollection<BsonDocument>("InvoiceCreditedID");
                return (true, collection);
            }
            catch 
            {
                return (false, null);
            }
        }
        public bool RecordID(string value)
        {
            var (connected,collection) = ConnectToDataBase();
            if (connected)
            {
                var document = new BsonDocument { { "ID", value } };
                collection.InsertOne(document);
                return true;
            }
            return false;
        }
        public List<string> ReadIDsList()
        {
            var (connected, collection) = ConnectToDataBase();
            if (connected)
            {
                var invoiceIDs = collection.Find(new BsonDocument()).ToList();

                return invoiceIDs
                    .Where(doc => doc.Contains("ID"))
                    .Select(doc => doc["ID"].AsString)
                    .ToList();
            }
            return new List<string>();
        }
        public bool AlreadyExists(string valueID)
        {
            var (connected, collection) = ConnectToDataBase();
            if (connected)
            {
                return ReadIDsList().Any(x => x.Contains(valueID));
            }
            return false;
        }

        public bool DeleteDocument(string search)
        {
            var (connected, collection) = ConnectToDataBase();
            if (connected)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("ID", search);
                collection.DeleteOne(filter);
                return true;
            }
            return false;
        }
    }
}
