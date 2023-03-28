using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DepotShopModels.Models;
public class UserModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonId]
    public string _id {  get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Address Address { get; set; }
    public int __v { get; set; }
}
