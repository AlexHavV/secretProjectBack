using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
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

        [HttpGet]
        [Route("GetProductsCount")]
        public IActionResult GetProductsCount()
        {
            var numberOfProducts = _context.Products.AsQueryable().Count(x => !x.IsDeleted);
            var numberOfPages = Math.Ceiling((double)numberOfProducts / 50);


            return Ok(new { countProducts = numberOfProducts, countPages = numberOfPages } );
        }

        [HttpGet]
        [Route("SearchProducts")]
        public IActionResult SearchProducts(int page, string searchParam)
        {
            int elemCount = 50;
            int firstElem = (page) * elemCount;
            int lastElem = firstElem + elemCount;

            var query = _context.Products.AsQueryable();

            var model = query
                .OrderBy(x => x.Id)
                .Where(x => !x.IsDeleted);
            if (!String.IsNullOrEmpty(searchParam))
            {
                model = model.Where(x => x.ProductName.Contains(searchParam));
            }

            var queryRes = model
                .Take(lastElem).Skip(firstElem)
                .Include(x => x.ProductImages.OrderBy(y => y.Priority))
                .Select(x => _mapper.Map<ProductViewModel>(x)).ToList();

            return Ok(queryRes);
        }
        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProducts([FromForm] ProductAddModel product)
        {
            try
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

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
        }

        [HttpPost]
        [Route("GenRandomProducts")]
        public IActionResult GenRandomProducts(int count)
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

            return Ok();
        }

        [HttpPut]
        [Route("UpdateProductById")]
        public IActionResult UpdateProductById([FromForm] AppProduct product)
        {
            try
            {
                var elemToUpdate = _context.Products
                    .SingleOrDefault<AppProduct>(x => x.Id == product.Id);
                if (elemToUpdate == null)
                {
                    return BadRequest(new { noProduct = "There is no product with such id!" });
                }
                elemToUpdate.ProductName = product.ProductName;
                elemToUpdate.Description = product.Description;
                elemToUpdate.Price = product.Price;

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
        }

        [HttpDelete]
        [Route("TrueDeleteProductById")]
        public IActionResult TrueDeleteProductById([FromForm] int idToDelete)
        {
            try
            {
                var elementToDelete = new AppProduct() { Id = idToDelete };

                _context.Products.Attach(elementToDelete);
                _context.Products.Remove(elementToDelete);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
           
        }

        [HttpDelete]
        [Route("DeleteProductById")]
        public IActionResult DeleteProductById([FromForm] int idToDelete)
        {
            try
            {
                var elementToDelete = new AppProduct() { Id = idToDelete };

                _context.Products.Attach(elementToDelete);
                elementToDelete.IsDeleted = true;

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
        }
    }
}
