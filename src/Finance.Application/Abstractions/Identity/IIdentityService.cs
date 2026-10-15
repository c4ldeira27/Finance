using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Finance.Application.Abstractions.Identity
{
    public interface IIdentityService
    {
        public Task<RegisterResult> RegisterAsync(string email, string password);
        public Task<LoginResult> LoginAsync(string email, string password);
        public Task LogoutAsync();

    }
}
