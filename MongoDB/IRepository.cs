using System.Threading.Tasks;
public interface IRepository<T> where T : class {
    void Add(T item);
    Task AddAsync(T item);
    
}