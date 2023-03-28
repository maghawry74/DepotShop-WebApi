﻿using AutoMapper;
using DepotShopDataAccess.Repository;
using DepotShopModels.DTOs.Order;
using DepotShopModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepotShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    public IOrderRepository OrderRepository { get; }
    public IUserRepository UserRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IMapper Mapper { get; }

    public OrderController(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        OrderRepository = orderRepository;
        UserRepository = userRepository;
        ProductRepository = productRepository;
        Mapper = mapper;
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public IActionResult Get()
    {
        return Ok(OrderRepository.GetOrdersWithDetails());
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var Order = OrderRepository.GetOrderWithDetails(id);
        return (Order != null) ? Ok(Order) : NotFound();
    }

    [HttpPost(Name = "CreateOrder")]
    public IActionResult Post([FromBody] createOrderDTO order)
    {
        var user = UserRepository.GetOne(U => U._id == order.user);
        if (user == null)
            return BadRequest("User Not Found");
        var ProductsId = new List<int>();
        foreach (var pro in order.Products)
        {
            ProductsId.Add(pro.Product);
        }
        var products = ProductRepository.GetProducts(ProductsId);
        if (products.Count != ProductsId.Count)
            return BadRequest("Some or All Products Not Found");
        var NewOrder = Mapper.Map<OrderModel>(order);
        NewOrder.Delivered = false;
        NewOrder.createdAt = DateTime.Now;
        var AddedOrder = OrderRepository.Add(NewOrder);
        return CreatedAtRoute("CreateOrder", AddedOrder);
    }

    [HttpPatch("{id}")]
    [Authorize(Policy = "Admin")]
    public IActionResult Patch(string id)
    {
        var Result = OrderRepository.CompleteOrder(O => O._id == id);
        return (Result.ModifiedCount > 0) ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var Result = OrderRepository.Delete(O => O._id == id);
        return (Result.DeletedCount > 0) ? Ok() : NotFound();
    }
}
