using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Common.Types;
using Project.Core;

namespace Project.Identity
{
    public class UserManager
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<User> _userStore;

        public UserManager(IRepository<User> userStore, IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _userStore = userStore;
        }

        public async Task<User> GetUserByNameAsync(string userName)
         => (await _userStore.FindAsync(u => u.Name == userName)).FirstOrDefault();

        public async Task CreateAsync(User user, string password)
        {
            var existingUser = await GetUserByNameAsync(user.Name);
            if (existingUser != null) 
                throw new Exception("User already exists");

            user.SetPassword(password, _passwordHasher);
            await _userStore.AddAsync(user);
        }

        public async Task<bool> PasswordSignInAsync(string userEmail, string password)
        {
            var userToLogin = await GetUserByNameAsync(userEmail);

            if (userToLogin == null)
                return false;

            var passwordHashMatch = _passwordHasher.VerifyHashedPassword(userToLogin, userToLogin.PasswordHash, password);
            return passwordHashMatch == PasswordVerificationResult.Success;
        }
    }
}
