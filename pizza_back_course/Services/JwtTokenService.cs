using Microsoft.Extensions.Configuration;
using pizza_back_course.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace pizza_back_course.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public string CreateToken(User user)
        //{
        //    List<Claim> claims = new List<Claim>()
        //    {

        //    }
        //}
    }
}
