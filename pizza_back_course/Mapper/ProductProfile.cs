using AutoMapper;
using pizza_back_course.Context;
using pizza_back_course.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza_back_course.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductAddModel, Product>();
            CreateMap<Product, ProductViewModel>();
        }
    }
}
