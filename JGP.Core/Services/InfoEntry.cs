// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-27-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-27-2022
// ***********************************************************************
// <copyright file="InfoEntry.cs" company="JGP.Core">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Services;

using System.Text.Json.Serialization;

/// <summary>
///     Class InfoEntry.
/// </summary>
public class InfoEntry
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InfoEntry" /> class.
    /// </summary>
    public InfoEntry()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InfoEntry" /> class.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public InfoEntry(string key, string value)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    ///     Gets or sets the key.
    /// </summary>
    /// <value>The key.</value>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    ///     Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    ///     Class KeyConstants.
    /// </summary>
    public static class KeyConstants
    {
        /// <summary>
        ///     The affected total.
        /// </summary>
        public const string AffectedTotal = "AffectedTotal";
    }
}