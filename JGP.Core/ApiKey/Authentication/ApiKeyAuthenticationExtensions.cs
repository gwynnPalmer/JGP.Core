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

namespace JGP.Core.ApiKey.Authentication;

using Microsoft.AspNetCore.Authentication;
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
    ///     Adds the API key management.
    /// </summary>
    /// <param name="authenticationBuilder">The authentication builder.</param>
    /// <param name="apiKeyAuthenticationSettings">The apiKeyAuthenticationSettings.</param>
    /// <returns>AuthenticationBuilder.</returns>
    public static AuthenticationBuilder AddApiKeyManagement(this AuthenticationBuilder authenticationBuilder,
        ApiKeyAuthenticationSettings apiKeyAuthenticationSettings)
    {
        return authenticationBuilder.AddScheme<ApiKeyAuthenticationSettings, ApiKeyAuthenticationhandler>(
            ApiKeyAuthenticationSettings.DefaultScheme,
            options =>
            {
                options.ServiceId = apiKeyAuthenticationSettings.ServiceId;
                options.ServiceName = apiKeyAuthenticationSettings.ServiceName;
            });
    }

    /// <summary>
    ///     Adds the API key management.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddApiKeyManagement(this IServiceCollection services, IConfiguration configuration)
    {
        // Setup Settings.
        var apiKeyAuthenticationSettings = new ApiKeyAuthenticationSettings();
        configuration.GetSection(ApiKeyAuthenticationSettings.ConfigurationSectionName).Bind(apiKeyAuthenticationSettings);

        // Setup Environmental Variables.
        var serviceId = Environment.GetEnvironmentVariable("ServiceId");
        var serviceName = Environment.GetEnvironmentVariable("ServiceName");

        apiKeyAuthenticationSettings.ServiceId = string.IsNullOrEmpty(serviceName)
            ? apiKeyAuthenticationSettings.ServiceId
            : Guid.Parse(serviceId);

        apiKeyAuthenticationSettings.ServiceName = string.IsNullOrEmpty(serviceName)
            ? apiKeyAuthenticationSettings.ServiceName
            : serviceName;

        // Add Settings.
        services.AddSingleton(apiKeyAuthenticationSettings);

        // Setup KeyStore Context.
        var keyStoreConnectionString = configuration.GetConnectionString("KeyStore");
        services.AddDbContext<KeyStoreContext>(options => options.UseSqlServer(keyStoreConnectionString,
            builder => builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null)));

        // Setup Services.
        services.AddMemoryCache();
        services.AddTransient<IKeyStoreContext, KeyStoreContext>();
        services.AddTransient<IApiKeyCacheService, ApiKeyCacheService>();

        // Ensure Key exists!
        services.EnsureKeyExists(apiKeyAuthenticationSettings);
    }

    /// <summary>
    ///     Ensures the key exists.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="apiKeyAuthenticationSettings">The apiKeyAuthenticationSettings.</param>
    /// <exception cref="System.ArgumentException">ApiKeyAuthenticationSettings are not configured! ServiceName and ServiceId are required.</exception>
    /// <exception cref="System.ArgumentException">KeyStoreContext is not configured!</exception>
    private static void EnsureKeyExists(this IServiceCollection services, ApiKeyAuthenticationSettings apiKeyAuthenticationSettings)
    {
        if (string.IsNullOrEmpty(apiKeyAuthenticationSettings.ServiceName) || apiKeyAuthenticationSettings.ServiceId == Guid.Empty)
        {
            throw new ArgumentException("ApiKeyAuthenticationSettings are not configured! ServiceName and ServiceId are required.");
        }

        var context = services.BuildServiceProvider().GetService<IKeyStoreContext>();
        if (context == null)
        {
            throw new ArgumentException("KeyStoreContext is not configured!");
        }

        var service = context.Services.FirstOrDefault(k => k.ServiceId == apiKeyAuthenticationSettings.ServiceId);
        if (service != null) return;

        using var generator = new ApiKeyGenerator();
        service = new Service
        {
            ServiceId = apiKeyAuthenticationSettings.ServiceId,
            ServiceName = apiKeyAuthenticationSettings.ServiceName,
            ApiKey = generator.GenerateApiKey()
        };
        context.Services.Add(service);
        context.SaveChanges();
    }
}