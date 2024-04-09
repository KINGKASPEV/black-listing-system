using ISWBlacklist.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISWBlacklist.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result> LoginAsync(string email, string password);
        Task<Result> SetPasswordAsync(string email, string password, string confirmPassword);
    }
}
