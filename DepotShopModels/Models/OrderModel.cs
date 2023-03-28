
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DepotShopModels.Models;

public class OrderModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    public string _id { set; get; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string user { set; get; }
    public double Price { set; get; }
    public OrderProductModel[] Products { set; get; }
    public bool Delivered { set; get; }
    public DateTime createdAt { set; get; }
    public int __v { set; get; }
}
