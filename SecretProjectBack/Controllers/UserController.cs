using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SecretProjectBack.Entity.User;
using SecretProjectBack.Models;
using SecretProjectBack.Services;

namespace SecretProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager,
            IJwtTokenService jwtTokenService,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] UserAddModel model)
        {
            string fileName = "placeholder.png";

            if (model.Image != null)
            {
                var ext = Path.GetExtension(model.Image.FileName);
                fileName = Path.GetRandomFileName() + ext;

                var dir = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                using (var stream = System.IO.File.Create(dir))
                {
                    model.Image.CopyTo(stream);
                }

            }
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Image = fileName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { message = result.Errors });

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new
            {
                token = _jwtTokenService.CreateToken(user)
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] UserLoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            //if (!result.Succeeded)
            //    return BadRequest(new { invalidId = "Wrong credentials!" });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                return Ok(new
                {
                    token = _jwtTokenService.CreateToken(user)
                });
            }
            return BadRequest(new { noUser = "No such user!" });

        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromForm] UserModel newData)
        {
            var userToEdit = await _userManager.FindByIdAsync(newData.Id.ToString());

            if (userToEdit != null)
            {
                if (newData.Password != null)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userToEdit);
                    var changePassword =
                        await _userManager.ResetPasswordAsync(userToEdit, resetToken, newData.Password);
                }

                userToEdit.UserName = newData.UserName;
                userToEdit.Image = newData.Image;
                userToEdit.Email = newData.Email;
                userToEdit.PhoneNumber = newData.PhoneNumber;

                await _userManager.UpdateAsync(userToEdit);

                return Ok(new
                {
                    token = _jwtTokenService.CreateToken(userToEdit)
                });
            }
            return BadRequest(new { invalidid = "Wrong Id!" });
            

            
        }
    }
}
