namespace NZWalks.API.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> AuthenticateAsync(string username, string password);
    }
}
