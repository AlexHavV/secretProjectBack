using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pizza_back_course.Context;
using pizza_back_course.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza_back_course.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly EFContext _context;
        private readonly IMapper _mapper;
        public PizzaController(EFContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("listProduct")]
        public async Task<IActionResult> GetProductsList()
        {
            List<ProductViewModel> list = await _context.Products
                .AsQueryable()
                .Select(x => _mapper.Map<ProductViewModel>(x))
                .ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        [Route("addProduct")]
        public IActionResult AddProduct([FromForm]ProductAddModel model)
        {
            var entity = _mapper.Map<Product>(model);
            _context.Products.Add(entity);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("deleteProduct")]
        public IActionResult DeleteProduct([FromForm]int id)
        {
            var item = _context.Products.SingleOrDefault(x => x.Id == id);
            _context.Products.Remove(item);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("addUser")]
        public IActionResult AddUser([FromForm]UserAddModel model)
        {
            var entity = _mapper.Map<User>(model);
            _context.Users.Add(entity);
            _context.SaveChanges();
            return Ok();
        }

        //[HttpGet]
        //[Route("getUser")]
        //public IActionResult GetUser(int id)
        //{
        //    var  
        //    return Ok();
        //}

        //[HttpPost]
    }
}
