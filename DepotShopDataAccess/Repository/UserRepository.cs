using DepotShopModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
namespace DepotShopDataAccess.Repository;
public class UserRepository : IUserRepository
{
    public IMongoCollection<UserModel> _Users { get; set; }
    public UserRepository(IMongoClient mongoClient)
    {
        _Users = mongoClient.GetDatabase("DepotShop").GetCollection<UserModel>("users");
    }

    public async Task<UserModel> Add(UserModel entity)
    {
        await _Users.InsertOneAsync(entity);
        return entity;
    }

    public async Task<List<UserModel>> GetAll()
    {
        var Users = await _Users.FindAsync(_ => true);
        return Users.ToList();
    }


    public async Task<UserModel?> GetOne(Expression<Func<UserModel, bool>> filter)
            => await _Users.Find(filter).FirstOrDefaultAsync();

    public async Task<UpdateResult> Update(UserModel entity)
    {
        var UpdatedProperites = new BsonDocument();

        if (entity.FirstName != null)
            UpdatedProperites.Add("FirstName", entity.FirstName);
        if (entity.LastName != null)
            UpdatedProperites.Add("LastName", entity.LastName);
        if (entity.Email != null)
            UpdatedProperites.Add("Email", entity.Email);
        if (entity.Phone != null)
            UpdatedProperites.Add("Phone", entity.Phone);
        if (entity.Address?.City != null)
            UpdatedProperites.Add("Address.City", entity.Address.City);
        if (entity.Address?.governorate != null)
            UpdatedProperites.Add("Address.governorate", entity.Address.governorate);
        if (entity.Password != null)
            UpdatedProperites.Add("Password", entity.Password);
        var UpdateDoc = new BsonDocument()
                        .Add("$set", UpdatedProperites);
        var update = new UpdateDocument(UpdateDoc);
        return await _Users.UpdateOneAsync(U => U._id == entity._id, update);
    }

    public async Task<DeleteResult> Delete(Expression<Func<UserModel, bool>> filter)
            => await _Users.DeleteOneAsync(filter);

}
