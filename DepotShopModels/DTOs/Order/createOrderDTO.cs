using DepotShopModels.Models;


namespace DepotShopModels.DTOs.Order;

public class createOrderDTO
{
    public string user { set; get; }
    public double Price { set; get; }
    public OrderProductModel[] Products { set; get; }
}
