using API.DTOs;
using API.Extensions;
using Application;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerInit
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(DataContext context, UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService) : base(context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null) return Unauthorized();

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (result.Succeeded)
                {
                    return new UserDto
                    {
                        Email = user.Email,
                        Username = user.UserName,
                        ProfileImageUrl = user.ProfileImageUrl,
                        Token = _tokenService.CreateToken(user),
                    };
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            {

                if (await _userManager.Users.AnyAsync(u => u.Email == registerDto.Email)) return BadRequest("Email Taken");
                if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.UserName)) return BadRequest("Username Taken");

                User user = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    ProfileImageUrl = "https://i.imgur.com/9S91hcw.png",
                };

                var response = await _userManager.CreateAsync(user, registerDto.Password);

                if (response.Succeeded)
                {
                    return new UserDto
                    {
                        Email = user.Email,
                        Username = user.UserName,
                        ProfileImageUrl = user.ProfileImageUrl,
                        Token = _tokenService.CreateToken(user),
                    };
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}