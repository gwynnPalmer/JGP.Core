﻿// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-28-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-28-2022
// ***********************************************************************
// <copyright file="ActionReceiptExtensions.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Services.Extensions.Web
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    /// <summary>
    ///     Class ActionReceiptExtensions.
    /// </summary>
    public static class ActionReceiptExtensions
    {
        /// <summary>
        ///     The error key
        /// </summary>
        public const string ErrorKey = "errors";

        /// <summary>
        ///     The information key
        /// </summary>
        public const string InformationKey = "information";

        /// <summary>
        ///     The problem details key
        /// </summary>
        private const string ProblemDetailsKey = "problem-details";

        /// <summary>
        ///     The semantics key
        /// </summary>
        private const string SemanticsKey = "rfc7231";

        /// <summary>
        ///     Adds the information.
        /// </summary>
        /// <param name="problemDetails">The problem details.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>ProblemDetails.</returns>
        public static ProblemDetails AddInformation(this ProblemDetails problemDetails, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrEmpty(value)) return problemDetails;

            if (problemDetails.Extensions != null && problemDetails.Extensions.ContainsKey(InformationKey))
            {
                var infoEntries = problemDetails.Extensions[InformationKey] as List<InfoEntry>;
                infoEntries?.Add(new InfoEntry(key, value));
                return problemDetails;
            }

            if (problemDetails.Extensions != null)
            {
                problemDetails.Extensions[InformationKey] = new List<InfoEntry>
                {
                    new(key, value)
                };
            }

            return problemDetails;
        }

        /// <summary>
        ///     Adds the information.
        /// </summary>
        /// <param name="problemDetails">The problem details.</param>
        /// <param name="infoEntry">The information entry.</param>
        /// <returns>ProblemDetails.</returns>
        public static ProblemDetails AddInformation(this ProblemDetails problemDetails, InfoEntry? infoEntry)
        {
            if (infoEntry is null) return problemDetails;

            if (string.IsNullOrWhiteSpace(infoEntry.Key) || string.IsNullOrWhiteSpace(infoEntry.Value))
                return problemDetails;

            return problemDetails.AddInformation(infoEntry.Key, infoEntry.Value);
        }

        /// <summary>
        ///     Gets the errors.
        /// </summary>
        /// <param name="problemDetails">The problem details.</param>
        /// <returns>System.Nullable&lt;Dictionary&lt;System.String, System.String[]&gt;&gt;.</returns>
        public static Dictionary<string, string[]>? GetErrors(this ProblemDetails problemDetails)
        {
            if (!problemDetails.Extensions.ContainsKey(ErrorKey))
                return new Dictionary<string, string[]>();

            var json = JsonSerializer.Serialize(problemDetails.Extensions[ErrorKey]);
            return JsonSerializer.Deserialize<Dictionary<string, string[]>>(json);
        }

        /// <summary>
        ///     Gets the information.
        /// </summary>
        /// <param name="problemDetails">The problem details.</param>
        /// <returns>System.Nullable&lt;List&lt;InfoEntry&gt;&gt;.</returns>
        public static List<InfoEntry>? GetInformation(this ProblemDetails problemDetails)
        {
            if (!problemDetails.Extensions.ContainsKey(InformationKey)) return new List<InfoEntry>();

            var json = JsonSerializer.Serialize(problemDetails.Extensions[InformationKey]);
            return JsonSerializer.Deserialize<List<InfoEntry>>(json);
        }

        /// <summary>
        ///     Converts to <see cref="ActionReceipt"/>.
        /// </summary>
        /// <param name="problemDetails">The problem details.</param>
        /// <returns>ActionReceipt.</returns>
        public static ActionReceipt ToActionReceipt(this ProblemDetails problemDetails)
        {
            var outcome = problemDetails.Status switch
            {
                StatusCodes.Status200OK => ActionOutcome.Success,
                StatusCodes.Status404NotFound => ActionOutcome.NotFound,
                _ => ActionOutcome.Exception
            };

            var infoEntries = problemDetails.GetInformation();
            var totalPresent = int.TryParse(infoEntries?
                .First(item => item.Key == InfoEntry.KeyConstants.AffectedTotal).Value, out var affectedTotal);

            var receipt = ActionReceipt.GetDefaultReceipt();
            receipt.Outcome = outcome;
            receipt.AffectedTotal = totalPresent ? affectedTotal : 0;
            receipt.InfoEntries = infoEntries ?? new List<InfoEntry>();
            receipt.Errors = problemDetails.GetErrors() ?? new Dictionary<string, string[]>();

            return receipt;
        }

        /// <summary>
        ///     Converts to <see cref="IActionResult"/>.
        /// </summary>
        /// <param name="actionReceipt">The action receipt.</param>
        /// <returns>IActionResult.</returns>
        public static IActionResult ToActionResult(this ActionReceipt actionReceipt)
        {
            return actionReceipt.Outcome switch
            {
                ActionOutcome.Success => new OkObjectResult(actionReceipt.ToProblemDetails("Success")),
                ActionOutcome.NotFound => new NotFoundObjectResult(actionReceipt.ToProblemDetails("Not Found")),
                ActionOutcome.Exception => new ObjectResult(actionReceipt.ToProblemDetails("Exception"))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                },
                _ => new ObjectResult(actionReceipt.ToProblemDetails("Unknown Error"))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                }
            };
        }

        /// <summary>
        ///     Converts to <see cref="ProblemDetails"/>.
        /// </summary>
        /// <param name="actionReceipt">The action receipt.</param>
        /// <param name="title">The title.</param>
        /// <param name="type">The type.</param>
        /// <returns>ProblemDetails.</returns>
        public static ProblemDetails ToProblemDetails(this ActionReceipt actionReceipt, string? title = null,
            string? type = null)
        {
            var statusCode = actionReceipt.Outcome switch
            {
                ActionOutcome.Success => StatusCodes.Status200OK,
                ActionOutcome.NotFound => StatusCodes.Status404NotFound,
                ActionOutcome.Exception => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            return new ProblemDetails
            {
                Status = statusCode,
                Title = title ?? "No Title",
                Type = type ?? "problem-details"
            }.AddActionReceipt(actionReceipt);
        }

        /// <summary>
        ///     To action receipt as an asynchronous operation.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>A Task&lt;ActionReceipt&gt; representing the asynchronous operation.</returns>
        public static async Task<ActionReceipt?> ToActionReceiptAsync(this HttpResponseMessage response)
        {
            // TODO: Fix Linq bug.
            var content = await response.Content.ReadAsStringAsync();
            if (!content.Contains(ProblemDetailsKey) && !content.Contains(SemanticsKey))
            {
                return response.IsSuccessStatusCode
                    ? ActionReceipt.GetSuccessReceipt()
                    : null;
            }

            var actionDetails = JsonConvert.DeserializeObject<ActionDetails>(content);
            return actionDetails
                .ToProblemDetails()
                .ToActionReceipt();
        }

        /// <summary>
        ///     Adds the action receipt.
        /// </summary>
        /// <param name="problemDetails">The problem details.</param>
        /// <param name="actionReceipt">The action receipt.</param>
        /// <returns>ProblemDetails.</returns>
        private static ProblemDetails AddActionReceipt(this ProblemDetails problemDetails, ActionReceipt actionReceipt)
        {
            problemDetails.Extensions[ErrorKey] = actionReceipt.Errors;

            foreach (var infoEntry in actionReceipt.InfoEntries)
            {
                problemDetails.AddInformation(infoEntry);
            }

            return problemDetails;
        }

        /// <summary>
        ///     Adds the model validation errors.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="actionReceipt">The action receipt.</param>
        public static void AddModelValidationErrors(this ModelStateDictionary modelState, ActionReceipt actionReceipt)
        {
            foreach (var error in actionReceipt.Errors)
            {
                if (Guid.TryParse(error.Key, out _)) continue;

                var errorMessage = string.Join(", ", error.Value);
                modelState.AddModelError(error.Key, errorMessage);
            }
        }
    }
}