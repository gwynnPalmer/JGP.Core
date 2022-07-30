// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-29-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-29-2022
// ***********************************************************************
// <copyright file="Service.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Api.Storage
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
        public Service(string serviceName, string apiKey)
        {
            ApiKey = apiKey;
            ServiceId = Guid.NewGuid();
            ServiceName = serviceName;
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
        ///     Updates the specified service name.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="apiKey">The API key.</param>
        public void Update(string serviceName, string apiKey)
        {
            ApiKey = apiKey;
            ServiceName = serviceName;
        }
    }
}