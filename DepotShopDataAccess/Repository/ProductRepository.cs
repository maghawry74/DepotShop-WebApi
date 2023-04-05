using DepotShopModels.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace DepotShopDataAccess.Repository;
public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<ProductModel> _Products;
    public ProductRepository(IMongoClient mongoClient)
    {
        _Products = mongoClient.GetDatabase("DepotShop").GetCollection<ProductModel>("products");
    }
    public async Task<ProductModel> Add(ProductModel entity)
    {
        var Result = _Products.Aggregate()
            .Group(pro => 1, Product => new { maxId = Product.Max(row => row._id) })
            .FirstOrDefault();
        int MaxId = Result?.maxId ?? 0;
        entity._id = MaxId + 1;
        await _Products.InsertOneAsync(entity);
        return entity;
    }

    public async Task<DeleteResult> Delete(Expression<Func<ProductModel, bool>> filter)
            => await _Products.DeleteOneAsync(filter);

    public async Task<List<ProductModel>> GetAll()
    {
        var Products = await _Products.FindAsync(_ => true);
        return Products.ToList();
    }

    public async Task<ProductModel?> GetOne(Expression<Func<ProductModel, bool>> filter)
            => await _Products.Find(filter).FirstOrDefaultAsync();

    public async Task<List<ProductModel>> GetProducts(List<int> ProductsIDs)
    {
        var filter = Builders<ProductModel>.Filter.In(P => P._id, ProductsIDs);
        var Products = await _Products.FindAsync(filter);
        return Products.ToList();
    }

    public async Task<UpdateResult> Update(ProductModel entity)
    {
        var UpdatedProps = new BsonDocument();
        if (entity.ProductName != null)
            UpdatedProps.Add(new BsonElement("ProductName", entity.ProductName));
        if (entity.Price != 0)
            UpdatedProps.Add(new BsonElement("Price", entity.Price));
        if (entity.Description != null)
            UpdatedProps.Add(new BsonElement("Description", entity.Description));
        if (entity.Amount != 0)
            UpdatedProps.Add(new BsonElement("Amount", entity.Amount));
        if (entity.image != null)
            UpdatedProps.Add(new BsonElement("image", entity.image));
        if (entity.Category != null)
            UpdatedProps.Add(new BsonElement("Category", entity.Category));
        var updatedoc = new BsonDocument()
                        .Add("$set", UpdatedProps);
        var update = new UpdateDocument(updatedoc);
        return await _Products.UpdateOneAsync(P => P._id == entity._id, update);
    }
}
