using Scanner.Core.DTOs;
using Scanner.Core.Models;
using Scanner.Helper.Response.Models;
using Scanner.Helper.Security.Hashing;
using Scanner.Helper.Security.JWT;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Service.Concrete
{

    public interface IAuthService
    {
        Task<User> Register(UserForRegisterDto userForRegisterDto);
        Task<User> Login(UserForLoginDto userForLoginDto);
        Task<bool> UserExists(string email);
        AccessToken CreateAccessToken(User user);
        void SaveRefreshToken(User user, AccessToken accessToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }

    public class AuthService : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        public AuthService(ITokenHelper tokenHelper, IUserService userService)
        {
            _tokenHelper = tokenHelper;
            _userService = userService;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            return _tokenHelper.GetPrincipalFromExpiredToken(token);
        }

        public void SaveRefreshToken(User user, AccessToken accessToken)
        {
            _userService.SaveRefreshToken(user.ID, accessToken.RefreshToken, accessToken.RefreshTokenExpiration);
        }

        public AccessToken CreateAccessToken(User user)
        {
            var accessToken = _tokenHelper.CreateToken(user);
            return accessToken;
        }

        public async Task<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = await _userService.GetByMailAddress(userForLoginDto.Email);

            if (userToCheck == null)
                throw new ApiException("Belirtilen kullanıcı adına uygun üyelik bulunamadı.", statusCode: (int)HttpStatusCode.NotFound);

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
                throw new ApiException("Kullanıcı adı veya şifre yanlış olduğu tespit edilmiştir.", statusCode: (int)HttpStatusCode.BadRequest);

            return userToCheck;
        }

        public async Task<User> Register(UserForRegisterDto userForRegisterDto)
        {
            byte[] passwordHash, passwordSalt;
            var userControl = await UserExists(userForRegisterDto.Email);

            if (!userControl) 
                throw new ApiException("Belirtilen mail adres ile üyelik oluşturulamaz.!", statusCode: (int)HttpStatusCode.NotFound);


            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

            var newUser = new User()
            {
                DeviceID = userForRegisterDto.DeviceID,
                Name = userForRegisterDto.Name,
                Surname = userForRegisterDto.Surname,
                Email = userForRegisterDto.Email,
                UserType = userForRegisterDto.UserType,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userService.AddAsync(newUser);
            return newUser;
        }

        public async Task<bool> UserExists(string email)
        {
            var result = await _userService.GetByMailAddress(email);
            if (result != null)
            {
                return false;
            }
            return true;
        }

    }
}
