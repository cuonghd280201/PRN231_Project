using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Models
{
    public partial class FUFlowerBouquetManagementContext : IdentityDbContext<IdentityUser>
    {
        public FUFlowerBouquetManagementContext()
        {
        }

        public FUFlowerBouquetManagementContext(DbContextOptions<FUFlowerBouquetManagementContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<FlowerBouquet> FlowerBouquets { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FlowerBouquet>(entity =>
            {
                entity.ToTable("FlowerBouquet");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description)
                    .HasMaxLength(220)
                    .IsUnicode(false);

                entity.Property(e => e.FlowerBouquetName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.FlowerBouquets)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__FlowerBou__Categ__2E1BDC42");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.FlowerBouquets)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__FlowerBou__Suppl__2F10007B");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.Property(e => e.Total).HasColumnType("money");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.FlowerBouquetId })
                    .HasName("PK__OrderDet__C983CA9F2B9D0931");

                entity.ToTable("OrderDetail");

                entity.HasOne(d => d.FlowerBouquet)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.FlowerBouquetId)
                    .HasConstraintName("FK__OrderDeta__Flowe__30F848ED");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__31EC6D26");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.SupplierAddress).HasMaxLength(150);

                entity.Property(e => e.SupplierName).HasMaxLength(50);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            var strConn = config["ConnectionStrings:ConnectionString"];
            return strConn;
        }
    }
}
