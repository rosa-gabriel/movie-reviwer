namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        string GetUsername();

        Task<bool> CheckIfCurrentUserIsAdmin();
    }
}