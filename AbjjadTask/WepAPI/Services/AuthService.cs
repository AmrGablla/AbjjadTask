using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WepAPI.Helpers;
using WepAPI.Interfaces;
using WepAPI.Models;
using WepAPI.Models.Settings;

namespace WepAPI.Services
{
    public class AuthService : IAuthService
    {
        private IUserService _userService;
        private readonly JWTSettings _jwtSettings;
        public AuthService(IUserService userService,
            IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _userService = userService;
        }

        public AuthenticationResponse AuthenticateAsync(AuthenticationRequest request)
        {
            var user = _userService.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new Exception($"No ApplicationUsers Registered with {request.UserName}.");
            }
            var password = StringCipher.Decrypt(user.PasswordHash, _jwtSettings.Key);

            if (password != request.Password)
            {
                throw new Exception($"Invalid Credentials for '{request.UserName}'.");
            }

            JwtSecurityToken jwtSecurityToken = GenerateJWToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
           
            response.UserName = user.UserName;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return response;
        }

        public bool RegisterAsync(RegisterRequest request)
        {
            if (request.Password == null ||
             request.ConfirmPassword == null)
            {
                throw new Exception($"One of this fields missing: Password, ConfirmPassword, Email, LastName, FirstName.");
            }
            if (request.Password.Length < 6)
            {
                throw new Exception($"password Minimum length 6.");
            }
            if (request.ConfirmPassword != request.Password)
            {
                throw new Exception($"Confirm Password wrong.");
            }

            var userWithSameUserName = _userService.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new Exception($"User name '{request.UserName}' is already taken.");
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                PasswordHash = StringCipher.Encrypt(request.Password, _jwtSettings.Key)
            };

            _userService.Create(user);
            return true;
        }

        private JwtSecurityToken GenerateJWToken(ApplicationUser user)
        {
            var claims = new[]
            { 
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

    }
}
