using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SecretProjectBack.Context;
using SecretProjectBack.Entity.Product;
using SecretProjectBack.Models;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult CartGetProduct([FromForm]CartGetProductsModel model)
        {
            var query = _context.Products.AsQueryable();

            var temp = query
                .Where(user => model.ArrayId.Contains(user.Id))
                .Include(x => x.ProductImages.OrderBy(y => y.Priority))
                .Select(x => _mapper.Map<ProductViewModel>(x)).ToList();


            return Ok(temp);
        }
    }
}
