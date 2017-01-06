using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbaySalesTracker
{
    public class OAuthTokenResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int AccessTokenExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpiresIn { get; set; }



    }
}