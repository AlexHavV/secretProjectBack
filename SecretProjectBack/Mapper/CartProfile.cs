using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SecretProjectBack.Entity.Cart;
using SecretProjectBack.Models;

namespace SecretProjectBack.Mapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartConfirmItemModel, AppCart>();
        }
    }
}
