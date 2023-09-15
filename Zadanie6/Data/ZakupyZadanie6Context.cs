using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zadanie6;
using Zadanie6.Models;

namespace Zadanie6.Data;

public partial class ZakupyZadanie6Context : DbContext
{
    public ZakupyZadanie6Context()
    {
    }

    public ZakupyZadanie6Context(DbContextOptions<ZakupyZadanie6Context> options)
        : base(options)
    {
    }

    public DbSet<Purchase> MyProperty { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.Property);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
