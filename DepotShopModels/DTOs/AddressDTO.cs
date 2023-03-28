using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepotShopModels.DTOs
{
    public class AddressDTO
    {
        [Required]
       public string City { get; set; }
        [Required]
        public string governorate { get; set; }
    }
}
