using DepotShopModels.DTOs.Order;
using DepotShopModels.Models;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DepotShopDataAccess.Repository
{
    public interface IOrderRepository : IRepository<OrderModel>
    {
        UpdateResult CompleteOrder(Expression<Func<OrderModel, bool>> filter);
        List<PopulatedOrderDTO> GetOrdersWithDetails();
        PopulatedOrderDTO? GetOrderWithDetails(string id);
    }
}
