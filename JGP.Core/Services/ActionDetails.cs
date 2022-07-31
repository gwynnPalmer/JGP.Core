// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-31-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-31-2022
// ***********************************************************************
// <copyright file="ActionDetails.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Services
{
    using System.Text.Json.Serialization;
    using Extensions.Web;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    ///     Class ActionDetails.
    ///     Implements the <see cref="ProblemDetails" />
    /// </summary>
    /// <seealso cref="ProblemDetails" />
    public class ActionDetails : ProblemDetails
    {
        /// <summary>
        ///     The extensions key
        /// </summary>
        private const string ExtensionsKey = "extensions";

        /// <summary>
        ///     Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        [JsonPropertyName("errors")]
        public Dictionary<string, string[]> Errors { get; set; } = new();

        /// <summary>
        ///     Gets or sets the information entries.
        /// </summary>
        /// <value>The information entries.</value>
        [JsonPropertyName("infoEntries")]
        public List<InfoEntry> InfoEntries { get; set; } = new();

        /// <summary>
        ///     Converts to problemdetails.
        /// </summary>
        /// <returns>ProblemDetails.</returns>
        public ProblemDetails ToProblemDetails()
        {
            var problemDetails = new ProblemDetails
            {
                Type = Type,
                Status = Status,
                Detail = Detail,
                Instance = Instance,
                Title = Title
            };

            var errors = new Dictionary<string, string[]>();
            var infoEntries = new List<InfoEntry>();

            if (Extensions.ContainsKey(ExtensionsKey))
            {
                var json = Extensions[ExtensionsKey].ToString();
                var extensions = JsonConvert.DeserializeObject<ActionDetails>(json);

                if (extensions.Errors.Any())
                {
                    foreach (var error in extensions.Errors)
                    {
                        errors.Add(error.Key, error.Value);
                    }
                }

                if (extensions.InfoEntries.Any())
                {
                    infoEntries.AddRange(extensions.InfoEntries);
                }
            }

            if (Errors.Any())
            {
                foreach (var error in Errors)
                {
                    errors.Add(error.Key, error.Value);
                }
            }

            if (InfoEntries.Any())
            {
                infoEntries.AddRange(InfoEntries);
            }

            if (errors.Any())
            {
                problemDetails.Extensions.Add(ActionReceiptExtensions.ErrorKey, errors);
            }

            if (infoEntries.Any())
            {
                problemDetails.Extensions.Add(ActionReceiptExtensions.InformationKey, infoEntries);
            }

            return problemDetails;
        }
    }
}