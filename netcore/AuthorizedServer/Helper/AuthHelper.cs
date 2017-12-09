using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthorizedServer.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AuthorizedServer.Helper
{
    /// <summary>Helper method for authentication </summary>
    public class AuthHelper
    {
        /// <summary>Get the access-token by username and password</summary>
        /// <param name="parameters"></param>
        /// <param name="_repo"></param>
        /// <param name="_settings"></param>
        public ResponseData DoPassword(Parameters parameters, IRTokenRepository _repo, IOptions<Audience> _settings)
        {
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var rToken = new RToken
            {
                ClientId = parameters.username,
                RefreshToken = refresh_token,
                Id = Guid.NewGuid().ToString(),
                IsStop = 0
            };
            if (_repo.AddToken(rToken).Result)
            {
                dynamic UserInfo = new System.Dynamic.ExpandoObject();
                UserInfo.FirstName = parameters.fullname;
                UserInfo.UserName = parameters.username;
                return new ResponseData
                {
                    Code = "999",
                    Message = "OK",
                    Content = UserInfo,
                    Data = GetJwt(parameters.username, refresh_token, _settings)
                };
            }
            else
            {
                return new ResponseData
                {
                    Code = "909",
                    Message = "can not add token to database",
                    Data = null
                };
            }
        }

        /// <summary>Get the access_token by refresh_token</summary>
        /// <param name="parameters"></param>
        /// <param name="_repo"></param>
        /// <param name="_settings"></param>
        public ResponseData DoRefreshToken(Parameters parameters, IRTokenRepository _repo, IOptions<Audience> _settings)
        {
            var token = _repo.GetToken(parameters.refresh_token, parameters.client_id).Result;
            if (token == null)
            {
                return new ResponseData
                {
                    Code = "905",
                    Message = "can not refresh token",
                    Data = null
                };
            }
            if (token.IsStop == 1)
            {
                return new ResponseData
                {
                    Code = "906",
                    Message = "refresh token has expired",
                    Data = null
                };
            }
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            token.IsStop = 1;
            var updateFlag = _repo.ExpireToken(token).Result;
            var addFlag = _repo.AddToken(new RToken
            {
                ClientId = parameters.client_id,
                RefreshToken = refresh_token,
                Id = Guid.NewGuid().ToString(),
                IsStop = 0
            });
            if (updateFlag && addFlag.Result)
            {
                return new ResponseData
                {
                    Code = "999",
                    Message = "OK",
                    Data = GetJwt(parameters.client_id, refresh_token, _settings)
                };
            }
            else
            {
                return new ResponseData
                {
                    Code = "910",
                    Message = "can not expire token or a new token",
                    Data = null
                };
            }
        }

        /// <summary>Get JWT</summary>
        /// <param name="client_id"></param>
        /// <param name="refresh_token"></param>
        /// <param name="_settings"></param>
        public string GetJwt(string client_id, string refresh_token, IOptions<Audience> _settings)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, client_id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };
            var symmetricKeyAsBase64 = _settings.Value.Secret;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var jwt = new JwtSecurityToken(
                issuer: _settings.Value.Iss,
                audience: _settings.Value.Aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(1)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(1).TotalSeconds,
                refresh_token = refresh_token,
            };
            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}
