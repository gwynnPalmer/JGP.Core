// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="IKeyStoreContext.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Api.Storage;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

/// <summary>
///     Interface IKeyStoreContext
///     Implements the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IKeyStoreContext : IDisposable
{
    /// <summary>
    ///     Saves the changes.
    /// </summary>
    /// <returns>System.Int32.</returns>
    int SaveChanges();

    /// <summary>
    ///     Saves the changes.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">if set to <c>true</c> [accept all changes on success].</param>
    /// <returns>System.Int32.</returns>
    int SaveChanges(bool acceptAllChangesOnSuccess);

    /// <summary>
    ///     Saves the changes asynchronous.
    /// </summary>
    /// <param name="cancellationToken">
    ///     The cancellation token that can be used by other objects or threads to receive notice
    ///     of cancellation.
    /// </param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Saves the changes asynchronous.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">if set to <c>true</c> [accept all changes on success].</param>
    /// <param name="cancellationToken">
    ///     The cancellation token that can be used by other objects or threads to receive notice
    ///     of cancellation.
    /// </param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

    /// <summary>
    ///     Entries the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <param name="entity">The entity.</param>
    /// <returns>EntityEntry&lt;TEntity&gt;.</returns>
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    ///     Entries the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>EntityEntry.</returns>
    EntityEntry Entry(object entity);

    /// <summary>
    ///     Gets or sets the services.
    /// </summary>
    /// <value>The services.</value>
    DbSet<Service> Services { get; set; }
}