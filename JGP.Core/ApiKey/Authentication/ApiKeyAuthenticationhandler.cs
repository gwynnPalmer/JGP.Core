// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ApiKeyAuthenticationhandler.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.ApiKey.Authentication
{
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Storage;

    /// <summary>
    ///     Class ApiKeyAuthenticationhandler.
    ///     Implements the
    ///     <see
    ///         cref="Microsoft.AspNetCore.Authentication.AuthenticationHandler{JGP.Core.Api.Authentication.ApiKeyAuthenticationSettings}" />
    /// </summary>
    /// <seealso
    ///     cref="Microsoft.AspNetCore.Authentication.AuthenticationHandler{JGP.Core.Api.Authentication.ApiKeyAuthenticationSettings}" />
    public class ApiKeyAuthenticationhandler : AuthenticationHandler<ApiKeyAuthenticationSettings>
    {
        /// <summary>
        ///     The key cache service
        /// </summary>
        private readonly IApiKeyCacheService _keyCacheService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiKeyAuthenticationhandler" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="encoder">The encoder.</param>
        /// <param name="clock">The clock.</param>
        /// <param name="keyCacheService">The key cache service.</param>
        public ApiKeyAuthenticationhandler(IOptionsMonitor<ApiKeyAuthenticationSettings> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IApiKeyCacheService keyCacheService)
            : base(options, logger, encoder, clock)
        {
            _keyCacheService = keyCacheService;
        }

        /// <summary>
        ///     Handle authenticate as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;AuthenticateResult&gt; representing the asynchronous operation.</returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyAuthenticationSettings.HeaderName, out var apiKeyHeaderValues))
            {
                return AuthenticateResult.NoResult();
            }

            var apiKey = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(apiKey))
            {
                return AuthenticateResult.NoResult();
            }

            var service = await _keyCacheService.GetServiceAsync(Options.ServiceId, Options.ServiceName);
            var authenticated = service.ApiKey == apiKey;

            if (!authenticated)
            {
                return await Task.Run(() => AuthenticateResult.Fail("Invalid Api Key"));
            }

            var claims = new List<Claim>();

            //TODO: Claims.

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return await Task.Run(() => AuthenticateResult.Success(ticket));
        }
    }
}