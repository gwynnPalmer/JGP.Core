// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ApiKeyConstants.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.ApiKey.Authentication;

/// <summary>
///     Class ApiKeyConstants.
/// </summary>
public class ApiKeyConstants
{
    /// <summary>
    ///     The default scheme
    /// </summary>
    public const string DefaultScheme = "API Key";

    /// <summary>
    ///     The API key header name
    /// </summary>
    public const string ApiKeyHeaderName = "X-Api-Key";
}