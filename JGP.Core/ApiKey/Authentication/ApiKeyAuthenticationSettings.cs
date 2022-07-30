// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ApiKeyAuthenticationSettings.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.ApiKey.Authentication;

using Microsoft.AspNetCore.Authentication;

/// <summary>
///     Class ApiKeyAuthenticationSettings.
/// </summary>
public class ApiKeyAuthenticationSettings : AuthenticationSchemeOptions
{
    /// <summary>
    ///     The default scheme
    /// </summary>
    public const string DefaultScheme = "API Key";

    /// <summary>
    ///     The configuration section name
    /// </summary>
    public const string ConfigurationSectionName = "JGPKeyAuth";

    /// <summary>
    ///     The header name
    /// </summary>
    public const string HeaderName = "X-Api-Key";

    /// <summary>
    ///     Gets or sets the service identifier.
    /// </summary>
    /// <value>The service identifier.</value>
    public Guid ServiceId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the service.
    /// </summary>
    /// <value>The name of the service.</value>
    public string ServiceName { get; set; }
}