using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SecretProjectBack.Entity.Cart;
using SecretProjectBack.Entity.Product;
using SecretProjectBack.Models;

namespace SecretProjectBack.Mapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartGeneralModel, AppCart>();
            CreateMap<AppProduct, CartViewModel>()
                .ForMember(x => x.Images, y => y.MapFrom(src => src.ProductImages));
        }
    }
}
