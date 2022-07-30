// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ServiceRecord.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.ApiKey.Storage;

/// <summary>
///     Class ServiceRecord.
/// </summary>
public class ServiceRecord
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceRecord" /> class.
    /// </summary>
    /// <param name="service">The service.</param>
    public ServiceRecord(Service service)
    {
        ServiceId = service.ServiceId;
        ServiceName = service.ServiceName;
        ApiKey = service.ApiKey;
        Url = service.Url;
    }

    /// <summary>
    ///     Gets or sets the API key.
    /// </summary>
    /// <value>The API key.</value>
    public string ApiKey { get; }

    /// <summary>
    ///     Gets or sets the service identifier.
    /// </summary>
    /// <value>The service identifier.</value>
    public Guid ServiceId { get; }

    /// <summary>
    ///     Gets or sets the name of the service.
    /// </summary>
    /// <value>The name of the service.</value>
    public string ServiceName { get; }

    /// <summary>
    ///     Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    public string? Url { get; }
}