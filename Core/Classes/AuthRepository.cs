using System.Text;
using System.Threading.Tasks;
using GliwickiDzik.API.Models;
using GliwickiDzik.Models;
using Microsoft.EntityFrameworkCore;

namespace GliwickiDzik.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUserExist(string username)
        {
            if(await _context.UserModel.AnyAsync(u => u.Username == username))
                return true;

            return false;
        }

        public async Task<UserModel> Login(string username, string password)
        {
            var user = await _context.UserModel.FirstOrDefaultAsync(u => u.Username == username);

            if(user == null)
                return null;
            
            if(!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;
            
            return user;
        }

        public async Task<UserModel> Register(UserModel user, string password)
        {
            byte[] passwordHash, passwordSalt;

            GetPasswordHashAndSalt(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.UserModel.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void GetPasswordHashAndSalt(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for(int i = 0; i < computeHash.Length; i++ )
                {
                    if(computeHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<NewTraining> GetTrainingAsync(int id)
        {
            return await _context.NewTraining.Include(x => x.Exercises).FirstOrDefaultAsync();
        }
    }
}