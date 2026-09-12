using System.Linq.Expressions;

namespace LibraryManagement.API.Repositories.Interfaces;

// Generic base repository - each module's specific repository (IBookRepository, ILoanRepository, etc.)
// should extend this and add module-specific query methods rather than duplicating CRUD.
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<int> SaveChangesAsync();
}
