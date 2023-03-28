
using System.ComponentModel.DataAnnotations;

namespace DepotShopModels.DTOs.User;

public class createUserDTO
{
    [Required]
    [MaxLength(20)]
    [MinLength(3)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(20)]
    [MinLength(3)]
    public string LastName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public AddressDTO Address { get; set; }
}
