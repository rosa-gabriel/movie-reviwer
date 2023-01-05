using Application;
using Application.Core;
using Application.Interfaces;
using Application.temp;
using Domain;
using Domain.Responses;
using Domain.Views;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerInit
    {
        public AccountController(IMediator mediator) : base(mediator) { }

        //Return a token if the give LoginDto data given matches a real user, else returns a Unauthorized(401) response.
        [HttpPost("login")]
        public async Task<ActionResult<UserTokenView>> Login(LoginDto loginDto)
        {
            Result<UserTokenView> result = await this._mediator.Send(new Login.Query { loginDto = loginDto });
            return this.ResultHandler(result);
        }
        [Authorize]
        [HttpPost("comment/{movieId}")]
        public async Task<ActionResult> PostComment(Guid movieId, Comment comment)
        {
            Result<Unit> result = await this._mediator.Send(new AddComment.Query { Comment = comment, movieId = movieId });
            return this.ResultHandler(result);
        }
        //Checks if the given token in a valid, returning a OK if yes or Unauthorized if not. 
        [Authorize]
        [HttpGet("check")]
        public async Task<ActionResult<UserTokenView>> CheckLogin()
        {
            Result<UserTokenView> result = await this._mediator.Send(new CheckCurentUser.Query());
            return this.ResultHandler(result);
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
        public async Task<ActionResult<UserTokenView>> Register(RegisterDto registerDto)
        {
            Result<UserTokenView> result = await this._mediator.Send(new Register.Query { registerDto = registerDto });
            return this.ResultHandler(result);
        }
        //Changes the favorite state for a given movie on the database.
        [Authorize]
        [HttpPost("favorite")]
        public async Task<ActionResult> PutFavorite(FavoriteView favoriteView)
        {
            Result<Unit> result = await this._mediator.Send(new FavoriteMovie.Query { MovieId = favoriteView.movieId, DesiredBool = favoriteView.desiredBool });
            return this.ResultHandler(result);
        }

        [Authorize]
        [HttpPut("settings/update")]
        public async Task<ActionResult<UserTokenView>> PutSettings(SettingsView newSettings)
        {
            Result<UserTokenView> result = await this._mediator.Send(new UpdateUserInfo.Query { NewSettings = newSettings });
            return this.ResultHandler(result);
        }

        [Authorize]
        [HttpGet("friends")]
        public async Task<ActionResult<FriendsListView>> GetFriends()
        {
            Result<FriendsListView> result = await this._mediator.Send(new ListProfileFriends.Query());
            return this.ResultHandler(result);
        }

        [Authorize]
        [HttpPost("friend/{id}")]
        public async Task<ActionResult<Unit>> PostFriend(string Id)
        {
            Result<Unit> result = await this._mediator.Send(new AddFriendRequest.Query { Id = Id });
            return this.ResultHandler(result);
        }

        [Authorize]
        [HttpDelete("remove/friend/{id}")]
        public async Task<ActionResult<Unit>> DeleteFriend(string Id)
        {
            Result<Unit> result = await this._mediator.Send(new RemoveFriend.Query { UserId = Id });
            return this.ResultHandler(result);
        }

        [Authorize]
        [HttpPut("confirm/friend/{id}")]
        public async Task<ActionResult<Unit>> PutFriend(Guid Id)
        {
            Result<Unit> result = await this._mediator.Send(new ConfirmFriendRequest.Query { FriendshipId = Id });
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
            Result<bool> result = await this._mediator.Send(new IsFavorite.Query { MovieId = id });
            return this.ResultHandler(result);
        }
        //Returns a list of Movies at the current page for the current user.
        [Authorize]
        [HttpGet("favorites/{name}/{page}")]
        public async Task<ActionResult<MoviePageView>> GetFavorites(string name, int page)
        {
            Result<MoviePageView> result = await this._mediator.Send(new ListFavoritesAtPage.Query { Username = name, Page = page });
            return this.ResultHandler(result);
        }

        [Authorize]
        [HttpGet("confirm/email/new")]
        public async Task<ActionResult<Unit>> SendNewEmailConfirm(string name, int page)
        {
            Result<Unit> result = await this._mediator.Send(new SendNewEmailConfirmRequest.Query {});
            return this.ResultHandler(result);
        }

        //Returns the profile info for a user with the given id.
        [HttpGet("profile/{id}")]
        public async Task<ActionResult<ProfileView>> GetProfile(string id)
        {
            Result<ProfileView> result = await this._mediator.Send(new FindProfile.Query { Id = id });
            return this.ResultHandler(result);
        }

        [Authorize]
        [HttpDelete("comment/{id}")]
        public async Task<ActionResult> DeletePerson(Guid id)
        {
            Result<Unit> result = await this._mediator.Send(new RemoveComment.Query { Id = id });
            return this.ResultHandler(result);
        }
        [Authorize]
        [HttpPut("comment")]
        public async Task<ActionResult> PutPerson(Comment NewComment)
        {
            Result<Unit> result = await this._mediator.Send(new UpdateComment.Query { NewComment = NewComment });
            return this.ResultHandler(result);
        }

        [HttpPut("confirm/email/{token}")]
        public async Task<ActionResult> PutConfirmEmail(string token)
        {
            Result<Unit> result = await this._mediator.Send(new ConfirmEmail.Query { Token = token });
            return this.ResultHandler(result);
        }
    }
}