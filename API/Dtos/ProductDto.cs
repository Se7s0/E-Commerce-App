using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; }  // Use string for ProductType
        public int ProductTypeId { get; set; }
        public string ProductBrand { get; set; } // Use string for ProductBrand
        public int ProductBrandId { get; set; }

    }
}