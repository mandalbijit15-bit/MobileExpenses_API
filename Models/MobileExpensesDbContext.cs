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

    public virtual DbSet<Transaction> Transactions { get; set; }

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

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Transactionid).HasName("transactions_pkey");

            entity.ToTable("transactions");

            entity.Property(e => e.Transactionid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("transactionid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Expenseamount)
                .HasPrecision(18, 2)
                .HasColumnName("expenseamount");
            entity.Property(e => e.Itemname)
                .HasMaxLength(255)
                .HasColumnName("itemname");
            entity.Property(e => e.Subcategoryid).HasColumnName("subcategoryid");

            entity.HasOne(d => d.Category).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_expenses_categories");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Subcategoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_expenses_subcategories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
