namespace JGP.Core.ApiKey.Storage;

/// <summary>
///     Interface IApiKeyCacheService
///     Implements the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IApiKeyCacheService : IDisposable
{
    /// <summary>
    ///     Gets the service asynchronous.
    /// </summary>
    /// <param name="serviceId">The service identifier.</param>
    /// <param name="serviceName">Name of the service.</param>
    /// <returns>ValueTask&lt;ServiceRecord&gt;.</returns>
    ValueTask<ServiceRecord> GetServiceAsync(Guid serviceId, string serviceName);

    /// <summary>
    ///     Gets the services asynchronous.
    /// </summary>
    /// <returns>ValueTask&lt;List&lt;ServiceRecord&gt;&gt;.</returns>
    ValueTask<List<ServiceRecord>> GetServicesAsync();
}