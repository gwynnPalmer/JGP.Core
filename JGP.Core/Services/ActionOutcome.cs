// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-27-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-27-2022
// ***********************************************************************
// <copyright file="ActionOutcome.cs" company="JGP.Core">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Services;

/// <summary>
///     Enum ActionOutcome
/// </summary>
public enum ActionOutcome
{
    /// <summary>
    ///     The success
    /// </summary>
    Success = 0,

    /// <summary>
    ///     The not found
    /// </summary>
    NotFound = 1,

    /// <summary>
    ///     The exception
    /// </summary>
    Exception = 2
}