// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="Service.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.ApiKey.Storage
{
    /// <summary>
    ///     Class Service.
    /// </summary>
    public class Service
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Service" /> class.
        /// </summary>
        public Service()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Service" /> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="url">The URL.</param>
        public Service(string serviceName, string apiKey, string? url = null)
        {
            ApiKey = apiKey;
            ServiceId = Guid.NewGuid();
            ServiceName = serviceName;
            Url = url;
        }

        /// <summary>
        ///     Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        public string ApiKey { get; set; }

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

        /// <summary>
        ///     Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string? Url { get; set; }

        /// <summary>
        ///     Sets the name.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public void SetName(string serviceName)
        {
            ServiceName = serviceName;
        }

        /// <summary>
        ///     Sets the API key.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public void SetApiKey(string apiKey)
        {
            ApiKey = apiKey;
        }

        /// <summary>
        ///     Sets the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void SetUrl(string? url)
        {
            Url = url;
        }
    }
}