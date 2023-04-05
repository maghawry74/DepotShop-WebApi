using DepotShopModels.Models;
namespace DepotShopDataAccess.Repository;
public interface IProductRepository : IRepository<ProductModel>
{
    Task<List<ProductModel>> GetProducts(List<int> ProductsIDs);
}
