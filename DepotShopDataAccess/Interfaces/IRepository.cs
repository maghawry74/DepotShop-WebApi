using DepotShopModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DepotShopDataAccess.Repository;
public interface IRepository <T> where T : class
{
    public List<T> GetAll();
    public T? GetOne(Expression<Func<T, bool>> filter);
    public T Add (T entity);
    public UpdateResult Update (T entity);
    public DeleteResult Delete (Expression<Func<T, bool>> filter);
}
