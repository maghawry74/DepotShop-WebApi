
using MongoDB.Bson.Serialization.Attributes;

namespace DepotShopModels.Models;

public class ProductModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int _id {  get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
    public string image { get; set; }
    public string Category { get; set; }
    public int __v { get; set; }
}
