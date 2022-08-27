// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 08-27-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 08-27-2022
// ***********************************************************************
// <copyright file="ActionReceiptModel.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Serialization;

using System.Text.Json.Serialization;
using Services;

/// <summary>
///     Class ActionReceiptModel.
/// </summary>
internal class ActionReceiptModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ActionReceiptModel" /> class.
    /// </summary>
    [JsonConstructor]
    public ActionReceiptModel()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ActionReceiptModel" /> class.
    /// </summary>
    /// <param name="actionReceipt">The action receipt.</param>
    public ActionReceiptModel(ActionReceipt actionReceipt)
    {
        AffectedTotal = actionReceipt.AffectedTotal;
        Errors = actionReceipt.Errors;
        InfoEntries = actionReceipt.InfoEntries;
        Outcome = actionReceipt.Outcome;
    }

    /// <summary>
    ///     Gets or sets the affected total.
    /// </summary>
    /// <value>The affected total.</value>
    [JsonPropertyName("affectedTotal")]
    public int AffectedTotal { get; set; }

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
    ///     Gets or sets the outcome.
    /// </summary>
    /// <value>The outcome.</value>
    [JsonPropertyName("outcome")]
    public ActionOutcome Outcome { get; set; }

    #region DOMAIN METHODS

    /// <summary>
    ///     Gets the action receipt.
    /// </summary>
    /// <returns>ActionReceipt.</returns>
    public ActionReceipt GetActionReceipt()
    {
        var actionReceipt = ActionReceipt.GetDefaultReceipt();

        actionReceipt.AffectedTotal = AffectedTotal;
        actionReceipt.Errors = Errors;
        actionReceipt.InfoEntries = InfoEntries;
        actionReceipt.Outcome = Outcome;

        return actionReceipt;
    }

    #endregion
}