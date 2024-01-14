using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public class ApiConfig : IApiConfig
    {
        /// <inheritdoc />
        public string AllowedOrigin { get; set; } = null!;

        /// <inheritdoc />
        public string TokenIssuer { get; set; } = null!;

        /// <inheritdoc />
        public string TokenAudience { get; set; } = null!;

        /// <inheritdoc />
        public string ApiSecret { get; set; } = null!;

        /// <inheritdoc />
        public string BaseUrlFrontend { get; set; } = null!;

        /// <inheritdoc />
        public int MinutesTillAuthorizationTokenExpires { get; set; }

        /// <inheritdoc />
        public int MinutesTillRefreshTokenExpires { get; set; }

        public string? BaseApiUrl { get; set; }

        public int MinutesTillCaseApplicationAuthorizationTokenExpires { get; set; } = 2;

        public int MinutesTillCaseApplicationRefreshTokenExpires { get; set; } = 60;
    }
}
