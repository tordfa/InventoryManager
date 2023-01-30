using BlazorApp1.Shared.Models;
using BlazorApp1.Server.DataAccess;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BlazorApp1.Server.Authentication
{
    public class JwtAuthenticationManager
    {
        public const string JWT_SECURITY_KEY = "securitykey123123123";
        public const int JWT_TOKEN_VALIDITY_MINS = 2000;

        private UserAccess _userAccess = new();
        public JwtAuthenticationManager()
        { 
        }

        // Check for password hash??
        public UserSessionModel? GenerateJwtToken(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            } 

            // Validating user credentials:
            var userCredentials = _userAccess.GetUserByUserName(username);
            if (userCredentials == null || userCredentials.Password != password)
            {
                return null;
            }

            // Generate JWT token
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim> 
            {
                new Claim(ClaimTypes.Name, userCredentials.UserName),
                new Claim(ClaimTypes.Role, userCredentials.Role)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature
                );
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenhandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenhandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenhandler.WriteToken(securityToken);

            var userSession = new UserSessionModel
            {
                UserName = username,
                Role = userCredentials.Role,
                Token = token,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };
            return userSession;
        }
        
    }
}
