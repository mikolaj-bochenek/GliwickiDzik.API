using System.Threading.Tasks;
using GliwickiDzik.API.Models;
using GliwickiDzik.Models;

namespace GliwickiDzik.Data
{
    public interface IAuthRepository
    {
        Task<UserModel> Login(string username, string password);
        Task<UserModel> Register(UserModel user, string password);
        Task<bool> IsUserExist(string username);
    }
}