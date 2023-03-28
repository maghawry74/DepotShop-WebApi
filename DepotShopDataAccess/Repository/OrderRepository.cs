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
    public OrderModel Add(OrderModel entity)
    {
        _Orders.InsertOne(entity);
        return entity;
    }

    public UpdateResult CompleteOrder(Expression<Func<OrderModel, bool>> filter)
    {
        var update = Builders<OrderModel>.Update.Set(O => O.Delivered, true);
        return _Orders.UpdateOne(filter, update);
    }

    public DeleteResult Delete(Expression<Func<OrderModel, bool>> filter)
            => _Orders.DeleteOne(filter);

    public List<OrderModel> GetAll()
            => _Orders.Find(_ => true).ToList();

    public OrderModel? GetOne(Expression<Func<OrderModel, bool>> filter)
            => _Orders.Find(filter).FirstOrDefault();

    public UpdateResult Update(OrderModel entity)
    {
        var UpdateProperties = new BsonDocument();
        var UpdateDoc = new BsonDocument().Add("$set", UpdateProperties);
        var update = new UpdateDocument(UpdateDoc);
        return _Orders.UpdateOne(order => order._id == entity._id, update);
    }
    public List<PopulatedOrderDTO> GetOrdersWithDetails()
            => GetOrders(_ => true);

    public PopulatedOrderDTO? GetOrderWithDetails(string id)
            => GetOrders(O => O._id == id).FirstOrDefault();



    private List<PopulatedOrderDTO> GetOrders(Expression<Func<OrderModel, bool>> filter)
            => _Orders.Aggregate()
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
            }).ToList();
}
