using Scanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Scanner.Helper.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string CreateRefreshToken();
    }
}
