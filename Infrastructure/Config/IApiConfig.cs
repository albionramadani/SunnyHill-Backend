using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public interface IApiConfig
    {
        /// <summary>
        /// Gets or sets the allowed origin, on which the client app is hosted (only on dev required, because clientapp is hosted on another url than the api)
        /// </summary>
        string AllowedOrigin { get; set; }

        /// <summary>
        /// The issuer of the token. Normally same as Audience
        /// </summary>
        string TokenIssuer { get; set; }

        /// <summary>
        /// Destination domain for which the token is created
        /// </summary>
        string TokenAudience { get; set; }

        /// <summary>
        /// Secret to generate Json authentication token
        /// </summary>
        string ApiSecret { get; set; }

        /// <summary>
        /// Base url to the frontend
        /// </summary>
        string BaseUrlFrontend { get; set; }

        /// <summary>
        /// Number of minutes, the authorizationToken is valid
        /// </summary>
        int MinutesTillAuthorizationTokenExpires { get; set; }

        /// <summary>
        /// Number of minutes, the refreshToken is valid
        /// </summary>
        int MinutesTillRefreshTokenExpires { get; set; }

        /// <summary>
        /// Number of minutes, the case application authorizationToken is valid
        /// </summary>
        int MinutesTillCaseApplicationAuthorizationTokenExpires { get; set; }

        /// <summary>
        /// Number of minutes, the case application refreshToken is valid
        /// </summary>
        int MinutesTillCaseApplicationRefreshTokenExpires { get; set; }


        /// <summary>
        /// Base Api Url
        /// </summary>
        public string? BaseApiUrl { get; set; }
    }
}
