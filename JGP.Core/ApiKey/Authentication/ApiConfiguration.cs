// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ApiConfiguration.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.ApiKey.Authentication
{
    using Storage;

    /// <summary>
    ///     Class ApiConfiguration.
    /// </summary>
    public class ApiConfiguration
    {
        /// <summary>
        ///     The default scheme
        /// </summary>
        public const string DefaultScheme = ApiKeyConstants.DefaultScheme;

        /// <summary>
        ///     Gets or sets the services.
        /// </summary>
        /// <value>The services.</value>
        public List<ServiceRecord> Services { get; set; } = new();
    }
}