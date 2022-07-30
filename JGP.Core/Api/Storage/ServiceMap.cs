// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 07-30-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 07-30-2022
// ***********************************************************************
// <copyright file="ServiceMap.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Api.Storage;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
///     Class ServiceMap.
///     Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{JGP.Core.Api.Storage.Service}" />
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{JGP.Core.Api.Storage.Service}" />
internal class ServiceMap : IEntityTypeConfiguration<Service>
{
    /// <summary>
    ///     Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        // Primary Key.
        builder.HasKey(x => x.ServiceId);

        // Properties.
        builder.Property(x => x.ServiceName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ApiKey)
            .IsRequired()
            .HasMaxLength(36);

        // Indexes.
        builder.HasIndex(x => x.ServiceName)
            .HasDatabaseName("IX_Service_ServiceName")
            .IsUnique();

        builder.HasIndex(service => service.ApiKey)
            .HasDatabaseName("IX_Service_ApiKey")
            .IsUnique();

        // Table & Column Mappings.
        builder.Property(x => x.ServiceId).HasColumnName("ServiceId");
        builder.Property(x => x.ServiceName).HasColumnName("ServiceName");
        builder.Property(x => x.ApiKey).HasColumnName("ApiKey");
    }
}