

using System.ComponentModel.DataAnnotations;

namespace DepotShopModels.DTOs.Product;

public class EditProductDTO
{
    public string ProductName { get; set; }
    public double? Price { get; set; }
    public string Description { get; set; }
    public int? Amount { get; set; }
    public string image { get; set; }
    public string Category { get; set; }
}
