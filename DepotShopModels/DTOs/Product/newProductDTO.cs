
using System.ComponentModel.DataAnnotations;

namespace DepotShopModels.DTOs.Product;

public class newProductDTO
{
    [Required]
    [StringLength(100)]
    public string ProductName { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public string image { get; set; }
    [Required]
    public string Category { get; set; }
}
