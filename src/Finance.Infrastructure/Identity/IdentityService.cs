using Finance.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.Identity
{
    public class IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IIdentityService
    {
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
                return LoginResult.Success;

            if (result.IsLockedOut)
                return LoginResult.LockedOut;

            return LoginResult.InvalidCredentials;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResult> RegisterAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(error => new AuthError(error.Code, error.Description))
                    .ToList();

                return RegisterResult.Failure(errors);
            }

            return RegisterResult.Success(user.Id);
        }
    }
}
