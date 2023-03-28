using AutoMapper;
using DepotShopModels.DTOs;
using DepotShopModels.DTOs.Order;
using DepotShopModels.DTOs.Product;
using DepotShopModels.DTOs.User;
using DepotShopModels.Models;

namespace DepotShop.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel,createUserDTO>()
                .ReverseMap();
            CreateMap<UserModel,EditUserDTO>()
                .ReverseMap();
            CreateMap<Address,AddressDTO>()
                .ReverseMap();
            CreateMap<ProductModel,newProductDTO>()
                .ReverseMap();
            CreateMap<ProductModel,EditProductDTO>()
                .ReverseMap();
            CreateMap<OrderModel,createOrderDTO>()
                .ReverseMap();

        }
    }
}
