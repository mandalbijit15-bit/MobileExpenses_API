using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MobileExpenses_API.Models;

public partial class MobileExpensesDbContext : DbContext
{
    public MobileExpensesDbContext()
    {
    }

    public MobileExpensesDbContext(DbContextOptions<MobileExpensesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("pk_category");

            entity.ToTable("categories");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedNever()
                .HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(100)
                .HasColumnName("categoryname");
            entity.Property(e => e.Isactive)
                .HasColumnType("bit(1)")
                .HasColumnName("isactive");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.Subcategoryid).HasName("pk_subcategory");

            entity.ToTable("subcategories");

            entity.Property(e => e.Subcategoryid)
                .ValueGeneratedNever()
                .HasColumnName("subcategoryid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Isactive)
                .HasColumnType("bit(1)")
                .HasColumnName("isactive");
            entity.Property(e => e.Subcategoryname)
                .HasMaxLength(100)
                .HasColumnName("subcategoryname");

            entity.HasOne(d => d.Category).WithMany(p => p.Subcategories)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("fk_subcategory_category");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
