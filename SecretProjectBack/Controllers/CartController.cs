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
            try
            {
                var queryCart = _context.Cart.AsQueryable()
                    .Join(
                        _context.Products,
                        Cart => Cart.ProductId,
                        Products => Products.Id,
                        (Cart, Products) => new { Cart, Products })
                    .Where(x => x.Cart.UserId == model.UserId && x.Cart.IsPayed == false);

                var result = queryCart.Select(x => _mapper.Map<CartViewModel>(x.Products)).ToList();

                result.ForEach((item) =>
                {
                    item.Amount = _context.Cart.AsQueryable().SingleOrDefault(x => x.ProductId == item.Id && x.UserId == model.UserId && x.IsPayed == false).Amount;

                    var imageQuery = _context.ProductImages.Where(x => x.ProductId == item.Id).ToList();

                    foreach (var image in imageQuery)
                    {
                        item.Images.Add(_mapper.Map<ProductImageModel>(image));
                    }
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
           

            
        }

        [HttpPost]
        [Route("CartAddProduct")]
        public IActionResult CartAddProduct([FromBody] CartGeneralModel model)
        {
            try
            {
                var query = _context.Cart.AsQueryable();
                var appearedProduct = query
                    .SingleOrDefault(x => x.UserId == model.UserId && x.ProductId == model.ProductId && x.IsPayed == false);

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
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            

            
        }

        [HttpPost]
        [Route("CartRemoveProduct")]
        public IActionResult CartRemoveProduct([FromBody]CartGeneralModel model)
        {
            var query = _context.Cart.AsQueryable();
            try
            {
                var productToRemove = query
                    .SingleOrDefault(x => x.UserId == model.UserId && x.ProductId == model.ProductId && x.IsPayed == false);
                _context.Cart.Remove(productToRemove);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
            
            
        }

        [HttpPost]
        [Route("ConfirmPurchase")]
        public IActionResult ConfirmPurchase([FromBody]CartConfirmOrderModel model)
        {
            try
            {
                var query = _context.Cart.AsQueryable();

                var allUserProducts = query
                    .Where(x => x.UserId == model.UserId && x.IsPayed == false);

                foreach (var item in allUserProducts)
                {
                    item.IsPayed = true;
                }

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
