using DepotShopModels.Models;
namespace DepotShopDataAccess.Repository;
public interface IProductRepository : IRepository<ProductModel>
{
    List<ProductModel> GetProducts(List<int> ProductsIDs);
}
