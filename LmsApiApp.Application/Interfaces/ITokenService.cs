
using LmsApiApp.Application.Settings;
using LmsApiApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(IList<string> roles, User user, JwtSetting jwtSetting);
        Task<Google.Apis.Auth.GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token);
        string GetToken(IList<string> roles, User user, JwtSetting jwtSetting);
    }
}
