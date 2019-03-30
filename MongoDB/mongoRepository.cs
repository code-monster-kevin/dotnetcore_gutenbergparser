using MongoDB.Driver;
using System.Threading.Tasks;
public abstract class MongoRepository<T> : IRepository<T> where T : class 
{
    private IMongoCollection<T> Collection { get; }
    private IMongoDatabase Database { get; }
    protected MongoRepository(IMongoContext context) 
    {
        Database = context.Database;
        Collection = Database.GetCollection<T>(typeof(T).Name);
    }

    public void Add(T item)
    {
        Collection.InsertOne(item);
    }

   public Task AddAsync(T item)
   {
       return Collection.InsertOneAsync(item);
   }
}