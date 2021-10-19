using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SecretProjectBack.Entity.Product;
using SecretProjectBack.Models;

namespace SecretProjectBack.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AppProduct, ProductViewModel>()
                .ForMember(x => x.Images, y => y.MapFrom(src => src.ProductImages));
            CreateMap<AppProductImage, ProductImageModel>()
                .ForMember( x => x.ProductImageName, y => y.MapFrom(src => src.Name));
            CreateMap<ProductAddModel, AppProduct>()
                .ForMember(product => product.DateCreated, opt => opt.MapFrom(x => DateTime.Now));
        }
    }
}
