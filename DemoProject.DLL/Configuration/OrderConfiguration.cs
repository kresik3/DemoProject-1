﻿using DemoProject.DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoProject.DLL.Configuration
{
  class OrderConfiguration : IEntityTypeConfiguration<Order>
  {
    public void Configure(EntityTypeBuilder<Order> builder)
    {
      builder.Property(x => x.DateOfCreation)
        .HasDefaultValueSql("GETUTCDATE()");

      builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(255);

      builder.Property(x => x.Mobile)
        .IsRequired()
        .HasMaxLength(20);

      builder.Property(x => x.Address)
        .IsRequired();
    }
  }
}