using JwtAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string TOKEN_KEY = "f1be1685d0d246ccb3e24c9bcd2109c8";
        public const int TOKEN_VALIDITY_MINS = 20;

        public AuthenticationResponse GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(TOKEN_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("Username",authenticationRequest.Username)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor 
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse{
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).Seconds,
                Token = token,
                Username = authenticationRequest.Username
            };
        }
    }
}
