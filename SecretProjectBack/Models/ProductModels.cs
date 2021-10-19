using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SecretProjectBack.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<ProductImageModel> Images { get; set; }
    }
    public class ProductImageModel
    {
        public string ProductImageName { get; set; }
    }
    public class ProductAddModel
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
    }
}
