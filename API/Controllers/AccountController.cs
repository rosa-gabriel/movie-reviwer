using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Application;
using Domain;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        protected readonly DataContext _context;

        public AccountController(DataContext context, UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService, IMediator mediator) : base(mediator)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenService = tokenService;
        }

        //Return a token if the give LoginDto data given matches a real user, else returns a Unauthorized(401) response.
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
                        Id = user.Id,
                    };
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Checks if the given token in a valid, returning a OK if yes or Unauthorized if not. 
        [Authorize]
        [HttpGet("check")]
        public ActionResult CheckLogin()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
        }
        //Registers a new user to the databae if the registerDto Email and username are unique, and the password matches the prerequisites.
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
                    ProfileImageUrl = "https://i.imgur.com/mggfJH8.png",
                    CreationDate = DateTime.Now,
                };

                var response = await _userManager.CreateAsync(user, registerDto.Password);

                if (response.Succeeded)
                {
                    return new UserDto
                    {
                        Id = user.Id,
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
        //Changes the favorite state for a given movie on the database.
        [Authorize]
        [HttpPost("favorite")]
        public async Task<ActionResult> PutFavorite(FavoriteResponse responseDto)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
                if (user == null) return BadRequest("Invalid user token");

                await this._mediator.Send(new FavoriteMovie.Query { user = user, MovieId = responseDto.movieId, DesiredBool = responseDto.desiredBool });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Returns if the given movie is favorited by the current user, or BadRequest(500) if false.
        [Authorize]
        [HttpGet("favorite/{id}")]
        public async Task<ActionResult<Boolean>> GetIsFavorite(Guid id)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
                if (user == null) return BadRequest("Invalid user token");

                bool isFavorite = await this._mediator.Send(new IsFavorite.Query { user = user, MovieId = id });
                return Ok(isFavorite);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Returns a list of Movies at the current page for the current user.
        [Authorize]
        [HttpGet("favorites/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetFavorites(int page)
        {
            try
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return await this._mediator.Send(new ListFavoritesAtPage.Query { UserId = id, Page = page });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //Returns the profile info for a user with the given id.
        [HttpGet("profile/{id}")]
        public async Task<ActionResult<ProfileResponse>> GetProfile(string id)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ProfileResponse profile;
                User user = await _userManager.FindByIdAsync(id);

                if (user == null) return BadRequest("User not found!");
                profile = new ProfileResponse(user);

                profile.RecentFavorites = await this._mediator.Send(new ListRecentFavorites.Query { user = user });
                profile.IsLogedIn = id.Equals(userId);

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}