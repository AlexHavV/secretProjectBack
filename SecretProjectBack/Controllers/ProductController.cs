using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretProjectBack.Context;
using SecretProjectBack.Entity.Product;
using SecretProjectBack.Models;

namespace SecretProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EFContext _context;
        private readonly IMapper _mapper;

        public ProductController(EFContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetProducts")]
        public IActionResult GetProducts()
        {
            var query = _context.Products.AsQueryable();
            var model = query
                .Where(x => !x.IsDeleted)
                .Include(x => x.ProductImages.OrderBy(y => y.Priority))
                .Select(x => _mapper.Map<ProductViewModel>(x)).ToList();

            return Ok(model);
        }

        [HttpPost]
        [Route("AddProducts")]
        public void AddProducts([FromForm] ProductAddModel product)
        {
            string fileName = "placeholder.png";

            if (product.Image != null)
            {
                var ext = Path.GetExtension(product.Image.FileName);
                fileName = Path.GetRandomFileName() + ext;

                var dir = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                using (var stream = System.IO.File.Create(dir))
                {
                    product.Image.CopyTo(stream);
                }

            }
            

            var entity = _mapper.Map<AppProduct>(product);
            _context.Products.Add(entity);
            _context.SaveChanges();

            var productImage = new AppProductImage()
            {
                Name = fileName,
                ProductId = entity.Id
            };
            _context.ProductImages.Add(productImage);
            _context.SaveChanges();
        }

        [HttpPost]
        [Route("GenRandomProducts")]
        public void GenRandomProducts(int count)
        {
            var rng = new Random();
            for (int i = 0; i < count; i++)
            {
                AddProducts(new ProductAddModel()
                {
                    ProductName = "Product FROM SWAGGER_" + rng.Next(0, 500),
                    Description = "Description FROM SWAGGER_" + rng.Next(0, 500),
                    Price = rng.Next(25, 50)
                });
            }
        }

        [HttpPut]
        [Route("UpdateProductById")]
        public void UpdateProductById([FromForm] AppProduct product)
        {
            var elemToUpdate = _context.Products
                .SingleOrDefault<AppProduct>(x => x.Id == product.Id);
            if (elemToUpdate == null)
            {
                return;
            }
            elemToUpdate.ProductName = product.ProductName;
            elemToUpdate.Description = product.Description;
            elemToUpdate.Price = product.Price;

            _context.SaveChanges();
        }

        [HttpDelete]
        [Route("TrueDeleteProductById")]
        public void TrueDeleteProductById([FromForm] int idToDelete)
        {
            var elementToDelete = new AppProduct() { Id = idToDelete };

            _context.Products.Attach(elementToDelete);
            _context.Products.Remove(elementToDelete);
            _context.SaveChanges();
        }

        [HttpDelete]
        [Route("DeleteProductById")]
        public void DeleteProductById([FromForm] int idToDelete)
        {
            var elementToDelete = new AppProduct() { Id = idToDelete };
           
            _context.Products.Attach(elementToDelete);
            elementToDelete.IsDeleted = true;

            _context.SaveChanges();
        }
    }
}
