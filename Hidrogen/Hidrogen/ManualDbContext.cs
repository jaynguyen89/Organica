using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hidrogen.Models {

    public partial class HidrogenDbContext {

        public IConfiguration Configuration { get; }

        public HidrogenDbContext(IConfiguration configuration) {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DevelopmentServer"));
        }
    }
}

//modelBuilder.Entity<RoleClaimer>(entity =>
//{
//    entity.Property(e => e.AllowCreate)
//        .IsRequired()
//        .HasDefaultValueSql("((1))");

//    entity.Property(e => e.AllowView)
//        .IsRequired()
//        .HasDefaultValueSql("((1))");

//    entity.Property(e => e.AllowEditOwn)
//        .IsRequired()
//        .HasDefaultValueSql("((1))");

//    entity.Property(e => e.AllowEditOthers)
//        .IsRequired()
//        .HasDefaultValueSql("((0))");

//    entity.Property(e => e.AllowDeleteOwn)
//        .IsRequired()
//        .HasDefaultValueSql("((1))");

//    entity.Property(e => e.AllowDeleteOthers)
//        .IsRequired()
//        .HasDefaultValueSql("((0))");

//    entity.Property(e => e.AllowReviveOwn)
//        .IsRequired()
//        .HasDefaultValueSql("((1))");

//    entity.Property(e => e.AllowReviveOthers)
//        .IsRequired()
//        .HasDefaultValueSql("((0))");

//    entity.Property(e => e.AllowTemporarily).HasMaxLength(255);

//    entity.HasOne(d => d.Hidrogenian)
//        .WithMany(p => p.RoleClaimer)
//        .HasForeignKey(d => d.HidrogenianId)
//        .HasConstraintName("FK_RoleClaimer_Hidrogenian");

//    entity.HasOne(d => d.Role)
//        .WithMany(p => p.RoleClaimer)
//        .HasForeignKey(d => d.RoleId)
//        .OnDelete(DeleteBehavior.ClientSetNull)
//        .HasConstraintName("FK_RoleClaimer_HidroRole");
//});