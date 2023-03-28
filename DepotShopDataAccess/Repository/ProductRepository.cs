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
    public ProductModel Add(ProductModel entity)
    {
        var Result = _Products.Aggregate()
            .Group(pro => 1, Product => new { maxId = Product.Max(row => row._id) })
            .FirstOrDefault();
        int MaxId = Result?.maxId ?? 0;
        entity._id = MaxId+1;
        _Products.InsertOne(entity);
        return entity;
    }

    public DeleteResult Delete(Expression<Func<ProductModel, bool>> filter)
            =>_Products.DeleteOne(filter);

    public List<ProductModel> GetAll()
            => _Products.Find(_ => true).ToList();

    public ProductModel? GetOne(Expression<Func<ProductModel, bool>> filter)
            =>_Products.Find(filter).FirstOrDefault();

    public List<ProductModel> GetProducts(List<int> ProductsIDs)
    {
        var filter = Builders<ProductModel>.Filter.In(P => P._id, ProductsIDs);
        return _Products.Find(filter).ToList();
    }

    public UpdateResult Update(ProductModel entity)
    {
        var UpdatedProps=new BsonDocument();
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
        return _Products.UpdateOne(P=>P._id==entity._id,update);
    }
}
