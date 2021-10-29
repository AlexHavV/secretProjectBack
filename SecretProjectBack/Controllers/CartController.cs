using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using SecretProjectBack.Context;
using SecretProjectBack.Entity.Product;
using SecretProjectBack.Models;
using Microsoft.EntityFrameworkCore;
using SecretProjectBack.Entity.Cart;

namespace SecretProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly EFContext _context;
        private readonly IMapper _mapper;

        public CartController(EFContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("CartGetProduct")]
        public IActionResult CartGetProduct([FromBody]CartGetProductsModel model)
        {
            var query = _context.Products.AsQueryable();

            List<int> listIds = new List<int>();

            foreach (var product in model.CartItems)
            {
                listIds.Add(product.Id);
            }


            var result = query
                .Where(product => listIds.Contains(product.Id))
                .Include(x => x.ProductImages.OrderBy(y => y.Priority))
                .Select(x => _mapper.Map<ProductViewModel>(x)).ToList();


            return Ok(result);
        }

        [HttpPost]
        [Route("CartAddProduct")]
        public IActionResult CartAddProduct([FromBody]CartAddModel model)
        {
            var query = _context.Cart.AsQueryable();
            var appearedProduct = query
                .SingleOrDefault(x => x.UserId == model.UserId && x.ProductId == model.ProductId);

            if (appearedProduct is null)
            {
                var entity = _mapper.Map<AppCart>(model);
                entity.Amount = 1M;
                _context.Cart.Add(entity);
            }
            else
            {
                appearedProduct.Amount++;
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
