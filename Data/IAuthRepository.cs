using System.Threading.Tasks;
using GliwickiDzik.Models;

namespace GliwickiDzik.Data
{
    public interface IAuthRepository
    {
        Task<User> Login(string username, string password);
        Task<User> Register(User user, string password);
        Task<bool> IsUserExist(string username);
    }
}