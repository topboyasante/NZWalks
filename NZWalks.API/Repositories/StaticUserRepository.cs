using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
            new User()
            {
                FirstName = "Nana", LastName = "Kwasi", EmailAddress="asantekwasi999@gmail.com",
                Id = Guid.NewGuid(), Username = "topboyasante", Password="kwasiA999",
                Roles = new List<string> {"reader"}
            },
            new User()
            {
                FirstName = "Erlich", LastName = "Bachmann", EmailAddress="ebachmann999@gmail.com",
                Id = Guid.NewGuid(), Username = "ebachmann", Password="bachmannE999",
                Roles = new List<string> {"reader","writer"}
            }
        };

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var user = Users.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && 
            x.Password == password);

            if(user != null)
            {
                return true;
            }
            return false;
        }
    }
}
