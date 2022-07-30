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
using Microsoft.Extensions.Options;
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
    ///     To be used by the API to register the required services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddApiKeyManagement(this IServiceCollection services, IConfiguration configuration)
    {
        var apiKeyAuthenticationSettings = configuration.ConfigureApiKeyAuthenticationSettings();

        // Add Settings.
        services.AddSingleton(apiKeyAuthenticationSettings);

        // Setup KeyStore Context.
        services.RegisterTransientKeyStoreContext(configuration);

        // Setup additional Services.
        services.AddMemoryCache();
        services.AddTransient<IApiKeyCacheService, ApiKeyCacheService>();

        // Ensure Key exists!
        services.EnsureKeyExists(apiKeyAuthenticationSettings);
    }

    /// <summary>
    ///     Registers the API key options.
    ///     To be used by the consumer of the API Services;
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void RegisterApiKeyOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterTransientKeyStoreContext(configuration);

        var context = services.GetKeyStoreContext();
        var records = context.Services.AsNoTracking().Select(service => new ServiceRecord(service)).ToList();
        services.AddSingleton(_ => Options.Create(new ApiConfiguration { Services = records }));
    }

    /// <summary>
    ///     Adds the service.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="apiKeyAuthenticationSettings">The API key authentication settings.</param>
    private static void AddService(this IKeyStoreContext context, ApiKeyAuthenticationSettings apiKeyAuthenticationSettings)
    {
        using var generator = new ApiKeyGenerator();
        var service = new Service
        {
            ServiceId = apiKeyAuthenticationSettings.ServiceId,
            ServiceName = apiKeyAuthenticationSettings.ServiceName,
            ApiKey = generator.GenerateApiKey()
        };
        context.Services.Add(service);
        context.SaveChanges();
    }

    /// <summary>
    ///     Configures the API key authentication settings.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>ApiKeyAuthenticationSettings.</returns>
    private static ApiKeyAuthenticationSettings ConfigureApiKeyAuthenticationSettings(this IConfiguration configuration)
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
        return apiKeyAuthenticationSettings;
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

        var context = services.GetKeyStoreContext();
        if (context.ServiceExists(apiKeyAuthenticationSettings)) return;

        context.AddService(apiKeyAuthenticationSettings);
    }

    /// <summary>
    /// Gets the key store context.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IKeyStoreContext.</returns>
    /// <exception cref="System.ArgumentException">KeyStoreContext is not configured!</exception>
    private static IKeyStoreContext GetKeyStoreContext(this IServiceCollection services)
    {
        var context = services.BuildServiceProvider().GetService<IKeyStoreContext>();
        if (context == null)
        {
            throw new ArgumentException("KeyStoreContext is not configured!");
        }

        return context;
    }

    /// <summary>
    ///     Registers the transient key store context.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    private static void RegisterTransientKeyStoreContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Setup KeyStore Context.
        var keyStoreConnectionString = configuration.GetConnectionString("KeyStore");
        services.AddDbContext<KeyStoreContext>(options => options.UseSqlServer(keyStoreConnectionString,
            builder => builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null)));
        services.AddTransient<IKeyStoreContext, KeyStoreContext>();
    }
    /// <summary>
    ///     Services the exists.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="apiKeyAuthenticationSettings">The API key authentication settings.</param>
    /// <returns><c>true</c> if Service Exists, <c>false</c> otherwise.</returns>
    private static bool ServiceExists(this IKeyStoreContext context, ApiKeyAuthenticationSettings apiKeyAuthenticationSettings)
    {
        var service = context.Services.FirstOrDefault(k => k.ServiceId == apiKeyAuthenticationSettings.ServiceId);
        return service != null;
    }
}