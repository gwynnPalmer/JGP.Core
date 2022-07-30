// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ApiKeyManagementMiddleware.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Api.Authentication
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    ///     Class ApiKeyManagementMiddleware.
    /// </summary>
    public class ApiKeyManagementMiddleware
    {
        /// <summary>
        ///     The key cache service
        /// </summary>
        private readonly IApiKeyCacheService _keyCacheService;

        /// <summary>
        ///     The next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        ///     The settings
        /// </summary>
        private readonly ApiKeyAuthenticationSettings _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiKeyManagementMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="keyCacheService">The key cache service.</param>
        /// <param name="settings">The settings.</param>
        public ApiKeyManagementMiddleware(RequestDelegate next, IApiKeyCacheService keyCacheService,
            ApiKeyAuthenticationSettings settings)
        {
            _next = next;
            _keyCacheService = keyCacheService;
            _settings = settings;
        }

        /// <summary>
        ///     Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_settings.HeaderName, out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var service = await _keyCacheService.GetServiceAsync(_settings.ServiceId, _settings.ServiceName);
            if (apiKey != service.ApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next.Invoke(context);
        }
    }
}