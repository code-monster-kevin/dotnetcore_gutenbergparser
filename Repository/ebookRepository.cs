public interface IEbookRepository : IRepository<Ebook> { }

public sealed class EbookRepository : MongoRepository<Ebook>, IEbookRepository
{
    public EbookRepository(DatabaseContext dbContext) : base(dbContext) { }
}