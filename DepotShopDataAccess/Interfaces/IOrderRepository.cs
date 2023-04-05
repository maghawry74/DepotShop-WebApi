using DepotShopModels.DTOs.Order;
using DepotShopModels.Models;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DepotShopDataAccess.Repository
{
    public interface IOrderRepository : IRepository<OrderModel>
    {
        Task<UpdateResult> CompleteOrder(Expression<Func<OrderModel, bool>> filter);
        Task<List<PopulatedOrderDTO>> GetOrdersWithDetails();
        Task<PopulatedOrderDTO?> GetOrderWithDetails(string id);
    }
}
