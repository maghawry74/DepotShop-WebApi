using MongoDB.Driver;
using System.Linq.Expressions;

namespace DepotShopDataAccess.Repository;
public interface IRepository<T> where T : class
{
    public Task<T> GetOne(Expression<Func<T, bool>> filter);
    public Task<List<T>> GetAll();
    public Task<T> Add(T entity);
    public Task<UpdateResult> Update(T entity);
    public Task<DeleteResult> Delete(Expression<Func<T, bool>> filter);
}
