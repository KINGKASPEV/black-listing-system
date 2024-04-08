using ISWBlacklist.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISWBlacklist.Application.Services.Interfaces
{
    public interface IJwtTokenGeneratorService
    {
        string GenerateToken(AppUser appUser);
    }
}
