using System.Security.Claims;
using API.DTOs;
using API.Extensions;
using Application;
using Application.Core;
using Application.Interfaces;
using Application.temp;
using Domain;
using Domain.Responses;
using Domain.Views;
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
        private readonly ITokenService _tokenService;
        protected readonly DataContext _context;

        public AccountController(DataContext context, UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IMediator mediator) : base(mediator)
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
                var user = await _userManager.FindByEmailAsync(loginDto.login);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(loginDto.login);
                    if (user == null) return Unauthorized();
                }

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

        [Authorize]
        [HttpPost("comment/{movieId}")]
        public async Task<ActionResult> PostComment(Guid movieId, Comment comment)
        {
            try
            {
                Result<Unit> result = await this._mediator.Send(new AddComment.Query { Comment = comment, movieId = movieId });
                return this.ResultHandler(result);
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

        [Authorize]
        [HttpGet("comments")]
        public async Task<ActionResult<List<CommentView>>> GetUserComments()
        {
            Result<List<CommentView>> result = await this._mediator.Send(new ListUserComments.Query());
            return this.ResultHandler(result);
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
                    Bio = "",
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

        [Authorize]
        [HttpPut("settings/update")]
        public async Task<ActionResult<UserDto>> PutSettings(SettingsView newSettings)
        {
            Result<UserDto> result = await this._mediator.Send(new UpdateUserInfo.Query { NewSettings = newSettings });
            return this.ResultHandler(result);
        }

        [Authorize]
        [HttpGet("settings")]
        public async Task<ActionResult> GetSettings(string id)
        {
            Result<SettingsView> result = await this._mediator.Send(new FindUserSettings.Query());
            return this.ResultHandler(result);
        }

        //Returns if the given movie is favorited by the current user, or BadRequest(500) if false.
        [Authorize]
        [HttpGet("favorite/{id}")]
        public async Task<ActionResult<Boolean>> GetIsFavorite(Guid id)
        {
            User user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null) return BadRequest("Invalid user token");

            Result<bool> result = await this._mediator.Send(new IsFavorite.Query { user = user, MovieId = id });
            return this.ResultHandler(result);
        }
        //Returns a list of Movies at the current page for the current user.
        [Authorize]
        [HttpGet("favorites/{page}")]
        public async Task<ActionResult<MoviePageResponse>> GetFavorites(int page)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Result<MoviePageResponse> result = await this._mediator.Send(new ListFavoritesAtPage.Query { UserId = id, Page = page });
            return this.ResultHandler(result);
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

                profile.RecentFavorites = (await this._mediator.Send(new ListRecentFavorites.Query { user = user })).Value;
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