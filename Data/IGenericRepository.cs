using System.Linq.Expressions;

namespace TourAndTravels.Data
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(Guid id);
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(Guid id);
        Task<bool> Delete(int id);
        Task<bool> Upsert(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}
