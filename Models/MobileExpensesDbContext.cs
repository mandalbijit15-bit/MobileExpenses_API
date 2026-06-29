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

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

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

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Rolename, "roles_rolename_key").IsUnique();

            entity.Property(e => e.Roleid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .HasColumnName("rolename");
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
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Category).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_expenses_categories");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Subcategoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_expenses_subcategories");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_transactions_users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Userid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("userid");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Monthlybalance)
                .HasPrecision(18, 2)
                .HasColumnName("monthlybalance");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Userrole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("Roleid")
                        .HasConstraintName("fk_userroles_role"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .HasConstraintName("fk_userroles_user"),
                    j =>
                    {
                        j.HasKey("Userid", "Roleid").HasName("pk_userroles");
                        j.ToTable("userroles");
                        j.IndexerProperty<int>("Userid").HasColumnName("userid");
                        j.IndexerProperty<int>("Roleid").HasColumnName("roleid");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
