using System.Threading.Tasks;
using GliwickiDzik.Models;

namespace GliwickiDzik.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public Task<bool> IsUserExist(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> Register(User user, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}