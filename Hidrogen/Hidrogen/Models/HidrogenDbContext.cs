using Microsoft.EntityFrameworkCore;

namespace Hidrogen.Models
{
    public partial class HidrogenDbContext : DbContext
    {
        public HidrogenDbContext()
        {
        }

        public HidrogenDbContext(DbContextOptions<HidrogenDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HidroAddress> HidroAddress { get; set; }
        public virtual DbSet<HidroProfile> HidroProfile { get; set; }
        public virtual DbSet<HidroRole> HidroRole { get; set; }
        public virtual DbSet<HidroSetting> HidroSetting { get; set; }
        public virtual DbSet<HidroTheme> HidroTheme { get; set; }
        public virtual DbSet<Hidrogenian> Hidrogenian { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<RoleClaimer> RoleClaimer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HidroAddress>(entity =>
            {
                entity.HasOne(d => d.Hidrogenian)
                    .WithMany(p => p.HidroAddress)
                    .HasForeignKey(d => d.HidrogenianId)
                    .HasConstraintName("FK_HidroAddress_Hidrogenian");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.HidroAddress)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HidroAddress_Location");
            });

            modelBuilder.Entity<HidroProfile>(entity =>
            {
                entity.Property(e => e.AvatarName).HasMaxLength(30);

                entity.Property(e => e.CitizenCardNumber).HasMaxLength(15);

                entity.Property(e => e.CitizenCardPhoto).HasMaxLength(70);

                entity.Property(e => e.Company).HasMaxLength(7);

                entity.Property(e => e.Ethnicity).HasMaxLength(7);

                entity.Property(e => e.FamilyName).HasMaxLength(30);

                entity.Property(e => e.GivenName).HasMaxLength(50);

                entity.Property(e => e.JobTitle).HasMaxLength(7);

                entity.Property(e => e.PersonalWebsite).HasMaxLength(100);

                entity.Property(e => e.SelfIntroduction).HasMaxLength(500);

                entity.HasOne(d => d.Hidrogenian)
                    .WithMany(p => p.HidroProfile)
                    .HasForeignKey(d => d.HidrogenianId)
                    .HasConstraintName("FK_HidroProfile_Hidrogenian");
            });

            modelBuilder.Entity<HidroRole>(entity =>
            {
                entity.Property(e => e.RoleDescription).HasMaxLength(100);

                entity.Property(e => e.RoleName).HasMaxLength(30);
            });

            modelBuilder.Entity<HidroSetting>(entity =>
            {
                entity.Property(e => e.HidroThemeId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.HidroTheme)
                    .WithMany(p => p.HidroSetting)
                    .HasForeignKey(d => d.HidroThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HidroSetting_HidroTheme");

                entity.HasOne(d => d.Hidrogenian)
                    .WithMany(p => p.HidroSetting)
                    .HasForeignKey(d => d.HidrogenianId)
                    .HasConstraintName("FK_HidroSetting_Hidrogenian");
            });

            modelBuilder.Entity<HidroTheme>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.BackgroundColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#FFFFFF')");

                entity.Property(e => e.BaseFontSize).HasDefaultValueSql("((20))");

                entity.Property(e => e.BaseFontWeight).HasDefaultValueSql("((300))");

                entity.Property(e => e.BaseMargin)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('15;10;15;10')");

                entity.Property(e => e.BaseOpacity).HasDefaultValueSql("((90))");

                entity.Property(e => e.BasePadding)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('20;15;20;15')");

                entity.Property(e => e.BaseRadius).HasDefaultValueSql("((4))");

                entity.Property(e => e.BaseShadow)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasDefaultValueSql("('0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24)')");

                entity.Property(e => e.BorderColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#49A9D6')");

                entity.Property(e => e.BorderWeight).HasDefaultValueSql("((1))");

                entity.Property(e => e.ColorDanger)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#EF81A8')");

                entity.Property(e => e.ColorSuccess)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#81EFA4')");

                entity.Property(e => e.ColorWarning)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#EFDD81')");

                entity.Property(e => e.HidroFont)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('KoHo')");

                entity.Property(e => e.HoveredShadow)
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasDefaultValueSql("('0 3px 6px rgba(0,0,0,0.16), 0 3px 6px rgba(0,0,0,0.23)')");

                entity.Property(e => e.LineHeight).HasDefaultValueSql("((25))");

                entity.Property(e => e.LinkDisabledColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#D5EBF6')");

                entity.Property(e => e.LinkHoveredColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#81CDEF')");

                entity.Property(e => e.LinkPrimaryColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#49A9D6')");

                entity.Property(e => e.ReservedColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#C981EF')");

                entity.Property(e => e.TextDisabledColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#999999')");

                entity.Property(e => e.TextHighlightedColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#49A9D6')");

                entity.Property(e => e.TextPrimaryColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#0D0D0D')");

                entity.Property(e => e.ThemeDisabledColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#FCEFE8')");

                entity.Property(e => e.ThemeHoveredColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#EA8F61')");

                entity.Property(e => e.ThemeName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDefaultValueSql("('Liquid Oxygen')");

                entity.Property(e => e.ThemePrimaryColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#81CDEF')");

                entity.Property(e => e.ThemeSecondaryColor)
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDefaultValueSql("('#EFA681')");
            });

            modelBuilder.Entity<Hidrogenian>(entity =>
            {
                entity.Property(e => e.AccessFailedCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.RecoveryToken).HasMaxLength(60);

                entity.Property(e => e.TwoFactorEnabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.AlternateAddress).HasMaxLength(50);

                entity.Property(e => e.BuildingName).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Commute).HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(30);

                entity.Property(e => e.District).HasMaxLength(30);

                entity.Property(e => e.Hamlet).HasMaxLength(30);

                entity.Property(e => e.Lane).HasMaxLength(10);

                entity.Property(e => e.Note).HasMaxLength(255);

                entity.Property(e => e.Postcode).HasMaxLength(10);

                entity.Property(e => e.Province).HasMaxLength(30);

                entity.Property(e => e.Quarter).HasMaxLength(30);

                entity.Property(e => e.StreetAddress).HasMaxLength(50);

                entity.Property(e => e.Suburb).HasMaxLength(50);

                entity.Property(e => e.Ward).HasMaxLength(30);
            });

            modelBuilder.Entity<RoleClaimer>(entity =>
            {
                entity.HasOne(d => d.Hidrogenian)
                    .WithMany(p => p.RoleClaimer)
                    .HasForeignKey(d => d.HidrogenianId)
                    .HasConstraintName("FK_RoleClaimer_Hidrogenian");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaimer)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleClaimer_HidroRole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
