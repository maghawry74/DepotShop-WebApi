using System;
using System.ComponentModel.DataAnnotations;

namespace DepotShopModels.DTOs;

public class LoginDTO
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
