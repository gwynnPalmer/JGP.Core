// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-27-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-27-2022
// ***********************************************************************
// <copyright file="ActionReceipt.cs" company="JGP.Core">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Services
{
    /// <summary>
    ///     Class ActionReceipt.
    /// </summary>
    public class ActionReceipt
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionReceipt" /> class.
        /// </summary>
        protected ActionReceipt()
        {
        }

        /// <summary>
        ///     Gets or sets the affected total.
        /// </summary>
        /// <value>The affected total.</value>
        public int AffectedTotal { get; set; }

        /// <summary>
        ///     Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public Dictionary<string, string[]> Errors { get; set; } = new();

        /// <summary>
        ///     Gets or sets the information entries.
        /// </summary>
        /// <value>The information entries.</value>
        public List<InfoEntry> InfoEntries { get; set; } = new();

        /// <summary>
        ///     Gets or sets the outcome.
        /// </summary>
        /// <value>The outcome.</value>
        public ActionOutcome Outcome { get; set; }
        /// <summary>
        ///     Gets the default receipt.
        /// </summary>
        /// <returns>ActionReceipt.</returns>
        public static ActionReceipt GetDefaultReceipt()
        {
            return new ActionReceipt();
        }

        /// <summary>
        ///     Gets the error receipt.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>ActionReceipt.</returns>
        public static ActionReceipt GetErrorReceipt(Exception exception)
        {
            var receipt = new ActionReceipt
            {
                Outcome = ActionOutcome.Exception,
                AffectedTotal = 0,
                InfoEntries = new List<InfoEntry>
                {
                    new(InfoEntry.KeyConstants.AffectedTotal, "0")
                }
            };

            receipt.AddException(exception);
            return receipt;
        }

        /// <summary>
        ///     Gets the error receipt.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ActionReceipt.</returns>
        public static ActionReceipt GetErrorReceipt(string message)
        {
            var receipt = new ActionReceipt
            {
                Outcome = ActionOutcome.Exception,
                AffectedTotal = 0,
                InfoEntries = new List<InfoEntry>
                {
                    new(InfoEntry.KeyConstants.AffectedTotal, "0")
                }
            };

            receipt.AddException(message);
            return receipt;
        }

        /// <summary>
        ///     Gets the not found receipt.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>ActionReceipt.</returns>
        public static ActionReceipt GetNotFoundReceipt(string? message = null)
        {
            var receipt = new ActionReceipt
            {
                Outcome = ActionOutcome.NotFound,
                AffectedTotal = 0,
                InfoEntries = new List<InfoEntry>
                {
                    new(InfoEntry.KeyConstants.AffectedTotal, "0")
                }
            };

            if (!string.IsNullOrWhiteSpace(message))
            {
                receipt.AddException(message);
            }

            return receipt;
        }

        /// <summary>
        ///     Gets the success receipt.
        /// </summary>
        /// <param name="affectedTotal">The affected total.</param>
        /// <returns>ActionReceipt.</returns>
        public static ActionReceipt GetSuccessReceipt(int affectedTotal = 0)
        {
            return new ActionReceipt
            {
                Outcome = ActionOutcome.Success,
                AffectedTotal = affectedTotal,
                InfoEntries = new List<InfoEntry>
                {
                    new(InfoEntry.KeyConstants.AffectedTotal, affectedTotal.ToString())
                }
            };
        }

        /// <summary>
        ///     Merges the specified action receipts.
        /// </summary>
        /// <param name="actionReceipts">The action receipts.</param>
        /// <returns>System.Nullable&lt;ActionReceipt&gt;.</returns>
        public static ActionReceipt? Merge(IEnumerable<ActionReceipt> actionReceipts)
        {
            var receipts = actionReceipts.ToArray();
            if (!receipts.Any())
            {
                return null;
            }

            if (receipts.Length == 1)
            {
                return receipts.First();
            }

            var initial = receipts[0];
            for (var i = 1; i < receipts.Length; i++)
            {
                initial.Merge(receipts[i]);
            }

            return initial;
        }

        /// <summary>
        ///     Adds the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void AddException(Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            Errors.Add($"{KeyConstants.Exception}-{Guid.NewGuid()}", new[] { exception.Message });
        }

        /// <summary>
        ///     Adds the exception.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddException(string message)
        {
            Errors.Add($"{KeyConstants.Exception}-{Guid.NewGuid()}", new[] { message });
        }

        /// <summary>
        ///     Adds the information.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddInfo(string key, string value)
        {
            InfoEntries.Add(new InfoEntry(key, value));
        }

        /// <summary>
        ///     Adds the information.
        /// </summary>
        /// <param name="infoEntry">The information entry.</param>
        public void AddInfo(InfoEntry infoEntry)
        {
            InfoEntries.Add(infoEntry);
        }

        /// <summary>
        ///     Gets the error messages.
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public IEnumerable<string> GetErrorMessages()
        {
            return Errors.Any()
                ? Errors.SelectMany(error => error.Value).ToArray()
                : Array.Empty<string>();
        }

        /// <summary>
        ///     Gets the error messages text.
        /// </summary>
        /// <returns>System.Nullable&lt;System.String&gt;.</returns>
        public string? GetErrorMessagesText()
        {
            if (!Errors.Any())
            {
                return null;
            }

            return Errors.Select(error => string.Join(". ", error.Value))
                .Aggregate(string.Empty, (current, message) => current + $" {message}")
                .Trim();
        }

        /// <summary>
        ///     Gets the information entry.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Nullable&lt;InfoEntry&gt;.</returns>
        public InfoEntry? GetInfoEntry(string key)
        {
            return InfoEntries.Exists(entry => entry.Key == key)
                ? InfoEntries.FirstOrDefault(entry => entry.Key == key)
                : null;
        }

        /// <summary>
        ///     Gets the information item value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Nullable&lt;System.String&gt;.</returns>
        public string? GetInfoItemValue(string key)
        {
            return InfoEntries.Exists(entry => entry.Key == key)
                ? InfoEntries.FirstOrDefault(entry => entry.Key == key)?.Value
                : null;
        }
        /// <summary>
        ///     Merges the specified action receipt.
        /// </summary>
        /// <param name="actionReceipt">The action receipt.</param>
        /// <returns>ActionReceipt.</returns>
        public ActionReceipt Merge(ActionReceipt actionReceipt)
        {
            foreach (var error in actionReceipt.Errors)
            {
                Errors.Add(error.Key, error.Value);
            }

            InfoEntries.AddRange(actionReceipt.InfoEntries);

            Outcome = Outcome switch
            {
                ActionOutcome.Success when actionReceipt.Outcome != ActionOutcome.Success => actionReceipt.Outcome,
                ActionOutcome.NotFound when actionReceipt.Outcome == ActionOutcome.Exception => ActionOutcome.Exception,
                _ => Outcome
            };
            return this;
        }
        /// <summary>
        ///     Class KeyConstants.
        /// </summary>
        private static class KeyConstants
        {
            /// <summary>
            ///     The exception
            /// </summary>
            public const string Exception = "Exception";
        }
    }
}