

using DepotShopModels.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DepotShopModels.DTOs.Order;

public class PopulatedOrderDTO
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { set; get; }
    public UserModel User { set; get; }
    public double Price { set; get; }
    public ProductModel[] Products { set; get; }
    public bool Delivered { set; get; }
    public DateTime createdAt { set; get; }

}
