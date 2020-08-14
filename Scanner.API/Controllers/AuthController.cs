using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scanner.Core.DTOs;
using Scanner.Helper.Response.Models;
using Scanner.Helper.Security.JWT;
using Scanner.Service.Concrete;

namespace Scanner.API.Controllers
{
    [Route("scanapi/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IUserService _userService;
        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ApiResponse> Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = await _authService.UserExists(userForRegisterDto.Email);

            if (!userExists)
                throw new ApiException("Belirtilen kullanıcı adı ile devam edilemez.", statusCode: (int)HttpStatusCode.BadRequest);

            var userToRegister = await _authService.Register(userForRegisterDto);

            if (userToRegister == null)
                throw new ApiException("Kullanıcı kayıt işleminde bir hata oluştu.", statusCode: (int)HttpStatusCode.BadRequest);

            var result = _authService.CreateAccessToken(userToRegister);
           
            if (result == null)
                throw new ApiException("Kullanıcı token işleminde bir hata oluştu.", statusCode: (int)HttpStatusCode.BadRequest);

            _authService.SaveRefreshToken(userToRegister, result);

            return new ApiResponse("Kullanıcı kayıt işlemi başarıyla tamamlanmıştır.", statusCode: (int)HttpStatusCode.Created, result: result);
        }

        [HttpPost("login")]
        public async Task<ApiResponse> Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = await _authService.Login(userForLoginDto);

            if (userToLogin == null)
                throw new ApiException("Belirtilen bilgiler ile giriş yapılamamaktadır.", statusCode: (int)HttpStatusCode.BadRequest);

            var result = _authService.CreateAccessToken(userToLogin);

            if (result == null)
                throw new ApiException("Kullanıcıya token oluşturma işleminde hata oluştuğu için giriş yapılamıyor.", statusCode: (int)HttpStatusCode.Unauthorized);

            //Kullanıcı giriş yaptığında access token ve refresh token tekrar üretip güncelliyor.
            _authService.SaveRefreshToken(userToLogin, result);

            return new ApiResponse("Kullanıcı girişi başarıyla yapılmıştır.", statusCode: (int)HttpStatusCode.OK, result: result);
        }

        [HttpPost("refreshtoken")]
        public async Task<ApiResponse> RefreshToken(AccessToken tokenResource)
        {
            if (tokenResource is null)
                throw new ApiException("Gönderilen bilgiler geçersiz. Lütfen kontrol ediniz.!", statusCode: (int)HttpStatusCode.BadRequest);

            string accessToken = tokenResource.Token;
            string refreshToken = tokenResource.RefreshToken;

            var principal = _authService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = await _userService.GetByMailAddress(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireDate <= DateTime.Now)
            {
                throw new ApiException("Kullanıcı refresh token süresi dolmuş.!", statusCode: (int)HttpStatusCode.Unauthorized);
            }

            var newAccessToken = _authService.CreateAccessToken(user);

            _authService.SaveRefreshToken(user, newAccessToken);

            return new ApiResponse("Kullanıcı girişi başarıyla yapılmıştır.", statusCode: (int)HttpStatusCode.OK, result: newAccessToken);
        }

    }
}
