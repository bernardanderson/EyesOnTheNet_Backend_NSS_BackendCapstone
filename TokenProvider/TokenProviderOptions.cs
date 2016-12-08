using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace EyesOnTheNet.TokenProvider
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";  // Sets the API pathname
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(1); // Sets the Auth TimeLimit
        public SigningCredentials SigningCredentials { get; set; }
    }
}
