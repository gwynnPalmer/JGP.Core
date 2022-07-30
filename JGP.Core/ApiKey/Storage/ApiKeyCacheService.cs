// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ApiKeyCacheService.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.ApiKey.Storage;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
///     Class ApiKeyCacheService.
/// Implements the <see cref="JGP.Core.ApiKey.Storage.IApiKeyCacheService" />
/// </summary>
/// <seealso cref="JGP.Core.ApiKey.Storage.IApiKeyCacheService" />
public class ApiKeyCacheService : IApiKeyCacheService
{
    /// <summary>
    ///     The context
    /// </summary>
    private readonly IKeyStoreContext _context;

    /// <summary>
    ///     The memory cache
    /// </summary>
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    ///     The service cache key
    /// </summary>
    private const string ServiceCacheKey = "JGP-Services";

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiKeyCacheService" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="memoryCache">The memory cache.</param>
    public ApiKeyCacheService(IKeyStoreContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    #region DISPOSAL

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _context?.Dispose();
    }

    #endregion

    /// <summary>
    ///     Get service as an asynchronous operation.
    /// </summary>
    /// <param name="serviceId">The service identifier.</param>
    /// <param name="serviceName">Name of the service.</param>
    /// <returns>A Task&lt;ServiceRecord&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentException">Service not found - serviceId</exception>
    public async ValueTask<ServiceRecord> GetServiceAsync(Guid serviceId, string serviceName)
    {
        var cacheKey = $"{serviceId}-{serviceName}";
        if (!_memoryCache.TryGetValue(cacheKey, out ServiceRecord record))
        {
            var service = await _context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServiceId == serviceId);

            if (service == null) throw new ArgumentException("Service not found", nameof(serviceId));
            record = new ServiceRecord(service);

            _memoryCache.Set(cacheKey, record);
        }

        return record;
    }

    /// <summary>
    ///     Get services as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentException">No Services Found</exception>
    public async ValueTask<List<ServiceRecord>> GetServicesAsync()
    {
        if (!_memoryCache.TryGetValue(ServiceCacheKey, out List<ServiceRecord> services))
        {
            services = await _context.Services
                .AsNoTracking()
                .Select(service => new ServiceRecord(service))
                .ToListAsync();

            if (!services.Any()) throw new ArgumentException("No Services Found");

            _memoryCache.Set(ServiceCacheKey, services);
        }

        return services;
    }
}