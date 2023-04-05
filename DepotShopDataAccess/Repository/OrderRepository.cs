using DepotShopModels.DTOs.Order;
using DepotShopModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DepotShopDataAccess.Repository;
public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<OrderModel> _Orders;
    public OrderRepository(IMongoClient mongoClient)
    {
        _Orders = mongoClient.GetDatabase("DepotShop").GetCollection<OrderModel>("orders");
    }
    public async Task<OrderModel> Add(OrderModel entity)
    {
        await _Orders.InsertOneAsync(entity);
        return entity;
    }

    public async Task<UpdateResult> CompleteOrder(Expression<Func<OrderModel, bool>> filter)
    {
        var update = Builders<OrderModel>.Update.Set(O => O.Delivered, true);
        return await _Orders.UpdateOneAsync(filter, update);
    }

    public async Task<DeleteResult> Delete(Expression<Func<OrderModel, bool>> filter)
            => await _Orders.DeleteOneAsync(filter);

    public async Task<List<OrderModel>> GetAll()
    {
        var orders = await _Orders.FindAsync(_ => true);
        return orders.ToList();
    }

    public async Task<OrderModel?> GetOne(Expression<Func<OrderModel, bool>> filter)
            => await _Orders.Find(filter).FirstOrDefaultAsync();

    public async Task<List<PopulatedOrderDTO>> GetOrdersWithDetails()
    {
        return await GetOrders(_ => true);
    }

    public async Task<PopulatedOrderDTO?> GetOrderWithDetails(string id)
    {
        var result = await GetOrders(O => O._id == id);
        return result.FirstOrDefault();
    }

    public async Task<UpdateResult> Update(OrderModel entity)
    {
        var UpdateProperties = new BsonDocument();
        var UpdateDoc = new BsonDocument().Add("$set", UpdateProperties);
        var update = new UpdateDocument(UpdateDoc);
        return await _Orders.UpdateOneAsync(order => order._id == entity._id, update);
    }
    private async Task<List<PopulatedOrderDTO>> GetOrders(Expression<Func<OrderModel, bool>> filter)

             => await _Orders.Aggregate()
            .Match(filter)
            .Lookup("users", "user", "_id", "User")
            .Lookup("products", "Products.Product", "_id", "Products")
            .Unwind("User")
            .Project<PopulatedOrderDTO>(new BsonDocument()
            {
                    { "Price", 1 },
                    { "Products", 1 },
                    { "Delivered", 1 },
                    { "createdAt", 1 },
                    {"User",1 },
            }).ToListAsync();
}
