using Microsoft.EntityFrameworkCore;
using WaterLibrary.Models;

namespace WaterLibrary.DbContexts
{
    public partial class WaterDbContext : DbContext
    {
        public WaterDbContext()
        {
        }

        public WaterDbContext(DbContextOptions<WaterDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Photos> Photos { get; set; }
        public virtual DbSet<Tokens> Tokens { get; set; }
        public virtual DbSet<Userphotos> Userphotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photos>(entity =>
            {
                entity.ToTable("photos");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Location)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.PhotoName)
                    .HasColumnType("varchar(150)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Tokens>(entity =>
            {
                entity.HasKey(e => e.TokenId)
                    .HasName("PRIMARY");

                entity.ToTable("tokens");

                entity.Property(e => e.TokenId).HasColumnType("int(11)");

                entity.Property(e => e.Life)
                    .IsRequired()
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'5'");

                entity.Property(e => e.Target)
                    .HasColumnType("varchar(70)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("'current_timestamp()'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.TokenString)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<Userphotos>(entity =>
            {
                entity.ToTable("userphotos");

                entity.HasIndex(e => e.PhotoId)
                    .HasName("userphotos_ibfk_1");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.HidrogenianId).HasColumnType("int(11)");

                entity.Property(e => e.IsAvatar).HasDefaultValueSql("'0'");

                entity.Property(e => e.IsCover).HasDefaultValueSql("'0'");

                entity.Property(e => e.PhotoId).HasColumnType("int(11)");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.Userphotos)
                    .HasForeignKey(d => d.PhotoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("userphotos_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
