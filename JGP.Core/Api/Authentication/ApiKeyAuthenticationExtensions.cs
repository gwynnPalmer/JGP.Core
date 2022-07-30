// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ApiKeyAuthenticationExtensions.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Api.Authentication;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage;

/// <summary>
///     Class ApiKeyAuthenticationExtensions.
/// </summary>
public static class ApiKeyAuthenticationExtensions
{
    /// <summary>
    ///     Users the API key management middleware.
    /// </summary>
    /// <param name="builder">The builder.</param>
    public static void UserApiKeyManagementMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ApiKeyManagementMiddleware>();
    }

    /// <summary>
    ///     Adds the API key management.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddApiKeyManagement(this IServiceCollection services, IConfiguration configuration)
    {
        // Setup Settings.
        var settings = new ApiKeyAuthenticationSettings();
        configuration.GetSection(ApiKeyAuthenticationSettings.ConfigurationSectionName).Bind(settings);

        // Setup Environmental Variables.
        var serviceId = Environment.GetEnvironmentVariable("ServiceId");
        var serviceName = Environment.GetEnvironmentVariable("ServiceName");

        settings.ServiceId = string.IsNullOrEmpty(serviceName)
            ? settings.ServiceId
            : Guid.Parse(serviceId);

        settings.ServiceName = string.IsNullOrEmpty(serviceName)
            ? settings.ServiceName
            : serviceName;

        // Add Settings.
        services.AddSingleton(settings);

        // Setup KeyStore Context.
        var keyStoreConnectionString = configuration.GetConnectionString("KeyStore");
        services.AddDbContext<KeyStoreContext>(options => options.UseSqlServer(keyStoreConnectionString,
            builder => builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null)));

        // Setup Services.
        services.AddMemoryCache();
        services.AddTransient<IKeyStoreContext, KeyStoreContext>();
        services.AddTransient<IApiKeyCacheService, ApiKeyCacheService>();
    }
}