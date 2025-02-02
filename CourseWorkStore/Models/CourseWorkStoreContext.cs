using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CourseWorkStore.Models;

public partial class CourseWorkStoreContext : DbContext
{
    public CourseWorkStoreContext()
    {
    }

    public CourseWorkStoreContext(DbContextOptions<CourseWorkStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress; Database=CourseWorkStore; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__2D10D14A884F85C3");

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.ProductDescription).HasColumnName("productDescription");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("productName");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("productPrice");
            entity.Property(e => e.ProductQuantity).HasColumnName("productQuantity");
            entity.Property(e => e.SupplierId).HasColumnName("supplierID");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Products_Suppliers");
        });

    modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__0261224C4C0A98F2");

            entity.Property(e => e.PurchaseId).HasColumnName("purchaseID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.PurchaseAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("purchaseAmount");
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("purchaseDate");
            entity.Property(e => e.PurchaseQuantity).HasColumnName("purchaseQuantity");

            entity.HasOne(d => d.Product).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchases_Products");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__DB8E62CD27D0A205");

            entity.Property(e => e.SupplierId).HasColumnName("supplierID");
            entity.Property(e => e.SupplierAddress).HasColumnName("supplierAddress");
            entity.Property(e => e.SupplierEmail)
                .HasMaxLength(255)
                .HasColumnName("supplierEmail");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(255)
                .HasColumnName("supplierName");
            entity.Property(e => e.SupplierPhone)
                .HasMaxLength(20)
                .HasColumnName("supplierPhone");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__CB9A1CDFCAD31C60");

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57239018B29").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Pass)
                .HasMaxLength(255)
                .HasColumnName("pass");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
