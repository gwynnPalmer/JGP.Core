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
///     Interface IApiKeyCacheService
///     Implements the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IApiKeyCacheService : IDisposable
{
    /// <summary>
    ///     Get service as an asynchronous operation.
    /// </summary>
    /// <param name="serviceId">The service identifier.</param>
    /// <param name="serviceName">Name of the service.</param>
    /// <returns>A Task&lt;Service&gt; representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Service not found - serviceId</exception>
    ValueTask<Service> GetServiceAsync(Guid serviceId, string serviceName);
}

/// <summary>
///     Class ApiKeyCacheService.
/// </summary>
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
    /// <returns>A Task&lt;Service&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentException">Service not found - serviceId</exception>
    public async ValueTask<Service> GetServiceAsync(Guid serviceId, string serviceName)
    {
        var cacheKey = $"{serviceId}-{serviceName}";
        if (!_memoryCache.TryGetValue(cacheKey, out Service service))
        {
            service = await _context.Services.FirstOrDefaultAsync(x => x.ServiceId == serviceId);
            if (service == null) throw new ArgumentException("Service not found", nameof(serviceId));

            _memoryCache.Set(cacheKey, service);
        }

        return service;
    }
}