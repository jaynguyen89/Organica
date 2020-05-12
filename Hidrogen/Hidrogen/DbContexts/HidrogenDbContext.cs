using Hidrogen.Models;
using Microsoft.EntityFrameworkCore;

namespace Hidrogen.DbContexts
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

        public virtual DbSet<BuyerRating> BuyerRating { get; set; }
        public virtual DbSet<BuyerReliability> BuyerReliability { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ChatGroup> ChatGroup { get; set; }
        public virtual DbSet<ChatMessage> ChatMessage { get; set; }
        public virtual DbSet<ChatParticipant> ChatParticipant { get; set; }
        public virtual DbSet<Classification> Classification { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<CountryRegion> CountryRegion { get; set; }
        public virtual DbSet<FineLocation> FineLocation { get; set; }
        public virtual DbSet<HidroAddress> HidroAddress { get; set; }
        public virtual DbSet<HidroProfile> HidroProfile { get; set; }
        public virtual DbSet<HidroRole> HidroRole { get; set; }
        public virtual DbSet<HidroSetting> HidroSetting { get; set; }
        public virtual DbSet<HidroStore> HidroStore { get; set; }
        public virtual DbSet<HidroTheme> HidroTheme { get; set; }
        public virtual DbSet<HidroTrader> HidroTrader { get; set; }
        public virtual DbSet<Hidrogenian> Hidrogenian { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemAsset> ItemAsset { get; set; }
        public virtual DbSet<ItemBasket> ItemBasket { get; set; }
        public virtual DbSet<ItemBundle> ItemBundle { get; set; }
        public virtual DbSet<ItemDetail> ItemDetail { get; set; }
        public virtual DbSet<ItemStock> ItemStock { get; set; }
        public virtual DbSet<ItemVariation> ItemVariation { get; set; }
        public virtual DbSet<Listing> Listing { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<RawLocation> RawLocation { get; set; }
        public virtual DbSet<RoleClaimer> RoleClaimer { get; set; }
        public virtual DbSet<SellerRating> SellerRating { get; set; }
        public virtual DbSet<SellerReliability> SellerReliability { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<StaffRole> StaffRole { get; set; }
        public virtual DbSet<StoreOwner> StoreOwner { get; set; }
        public virtual DbSet<StoreRole> StoreRole { get; set; }
        public virtual DbSet<StoreStaff> StoreStaff { get; set; }
        public virtual DbSet<Trading> Trading { get; set; }
        public virtual DbSet<TradingRating> TradingRating { get; set; }
        public virtual DbSet<Warranty> Warranty { get; set; }
        public virtual DbSet<WarrantyTerm> WarrantyTerm { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BuyerRating>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(200);

                entity.Property(e => e.RatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Reliability).HasMaxLength(200);

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.BuyerRatingBuyer)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuyerRating_HidroTrader_Buyer");

                entity.HasOne(d => d.RatedBy)
                    .WithMany(p => p.BuyerRatingRatedBy)
                    .HasForeignKey(d => d.RatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuyerRating_HidroTrader_Seller");

                entity.HasOne(d => d.Trading)
                    .WithMany(p => p.BuyerRating)
                    .HasForeignKey(d => d.TradingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuyerRating_Trading");
            });

            modelBuilder.Entity<BuyerReliability>(entity =>
            {
                entity.Property(e => e.VotedOn).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.BuyerReliability)
                    .HasForeignKey(d => d.RatingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuyerReliability_BuyerRating");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.AvatarName).HasMaxLength(100);

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Restrictions).HasMaxLength(250);

                entity.HasOne(d => d.Dependant)
                    .WithMany(p => p.InverseDependant)
                    .HasForeignKey(d => d.DependantId)
                    .HasConstraintName("FK_Category_Self");
            });

            modelBuilder.Entity<ChatGroup>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GroupAvatar).HasMaxLength(150);

                entity.Property(e => e.GroupTitle).HasMaxLength(50);
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.Property(e => e.Attachment).HasMaxLength(150);

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.Property(e => e.IsHiddenFor).HasMaxLength(500);

                entity.Property(e => e.SeenByIds).HasMaxLength(500);

                entity.Property(e => e.SentOn).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ChatMessage)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_ChatGroup");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.ChatMessage)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessage_ChatParticipant");
            });

            modelBuilder.Entity<ChatParticipant>(entity =>
            {
                entity.Property(e => e.TimeStamps).HasMaxLength(1000);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ChatParticipant)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatParticipant_ChatGroup");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.ChatParticipant)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatParticipant_Hidrogenian");
            });

            modelBuilder.Entity<Classification>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Classification)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Classification_Category");

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.Classification)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Classification_Listing");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Continent).HasMaxLength(30);

                entity.Property(e => e.CountryCode).HasMaxLength(5);

                entity.Property(e => e.CountryName).HasMaxLength(50);

                entity.Property(e => e.CurrencyCode).HasMaxLength(5);

                entity.Property(e => e.CurrencyName).HasMaxLength(30);
            });

            modelBuilder.Entity<CountryRegion>(entity =>
            {
                entity.Property(e => e.RegionCode).HasMaxLength(10);

                entity.Property(e => e.RegionName).HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CountryRegion)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CountryRegion_Country");

                entity.HasOne(d => d.ResidedIn)
                    .WithMany(p => p.InverseResidedIn)
                    .HasForeignKey(d => d.ResidedInId)
                    .HasConstraintName("FK_CountryRegion_Self");
            });

            modelBuilder.Entity<FineLocation>(entity =>
            {
                entity.Property(e => e.AlternateAddress).HasMaxLength(50);

                entity.Property(e => e.BuildingName).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(40);

                entity.Property(e => e.Commute).HasMaxLength(40);

                entity.Property(e => e.District).HasMaxLength(40);

                entity.Property(e => e.Group).HasMaxLength(30);

                entity.Property(e => e.Hamlet).HasMaxLength(40);

                entity.Property(e => e.Lane).HasMaxLength(10);

                entity.Property(e => e.PoBoxNumber).HasMaxLength(40);

                entity.Property(e => e.Postcode).HasMaxLength(10);

                entity.Property(e => e.Province).HasMaxLength(40);

                entity.Property(e => e.Quarter).HasMaxLength(40);

                entity.Property(e => e.State).HasMaxLength(40);

                entity.Property(e => e.StreetAddress).HasMaxLength(50);

                entity.Property(e => e.Suburb).HasMaxLength(50);

                entity.Property(e => e.Town).HasMaxLength(40);

                entity.Property(e => e.Ward).HasMaxLength(40);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.FineLocation)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FineLocation_Country");
            });

            modelBuilder.Entity<HidroAddress>(entity =>
            {
                entity.Property(e => e.LastUpdated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title).HasMaxLength(30);

                entity.HasOne(d => d.Hidrogenian)
                    .WithMany(p => p.HidroAddress)
                    .HasForeignKey(d => d.HidrogenianId)
                    .HasConstraintName("FK_HidroAddress_Hidrogenian");
            });

            modelBuilder.Entity<HidroProfile>(entity =>
            {
                entity.Property(e => e.AvatarInformation).HasMaxLength(1000);

                entity.Property(e => e.CitizenCardNumber).HasMaxLength(15);

                entity.Property(e => e.CitizenCardPhoto).HasMaxLength(70);

                entity.Property(e => e.Company).HasMaxLength(30);

                entity.Property(e => e.Ethnicity).HasMaxLength(30);

                entity.Property(e => e.FamilyName).HasMaxLength(30);

                entity.Property(e => e.GivenName).HasMaxLength(50);

                entity.Property(e => e.JobTitle).HasMaxLength(30);

                entity.Property(e => e.PersonalWebsite).HasMaxLength(100);

                entity.Property(e => e.SelfIntroduction).HasMaxLength(500);

                entity.HasOne(d => d.Hidrogenian)
                    .WithMany(p => p.HidroProfile)
                    .HasForeignKey(d => d.HidrogenianId)
                    .HasConstraintName("FK_HidroProfile_Hidrogenian");
            });

            modelBuilder.Entity<HidroRole>(entity =>
            {
                entity.Property(e => e.RoleDescription).HasMaxLength(250);

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

            modelBuilder.Entity<HidroStore>(entity =>
            {
                entity.Property(e => e.Introduction).HasMaxLength(4000);

                entity.Property(e => e.OpenedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StoreLogo).HasMaxLength(150);

                entity.Property(e => e.StoreName).HasMaxLength(70);
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

            modelBuilder.Entity<HidroTrader>(entity =>
            {
                entity.HasOne(d => d.Hidrogenian)
                    .WithMany(p => p.HidroTrader)
                    .HasForeignKey(d => d.HidrogenianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HidroBuyer_Hidrogenian");
            });

            modelBuilder.Entity<Hidrogenian>(entity =>
            {
                entity.Property(e => e.CookieToken).HasMaxLength(150);

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastDeviceInfo).HasMaxLength(150);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.RecoveryToken).HasMaxLength(60);

                entity.Property(e => e.TwoFaSecretKey).HasMaxLength(12);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ItemName).HasMaxLength(30);

                entity.Property(e => e.ItemPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.Item)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_Listing");
            });

            modelBuilder.Entity<ItemAsset>(entity =>
            {
                entity.Property(e => e.AssetLocation).HasMaxLength(150);

                entity.Property(e => e.AssetName).HasMaxLength(250);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemAsset)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemAsset_Item");
            });

            modelBuilder.Entity<ItemBasket>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.BasketNote).HasMaxLength(100);

                entity.HasOne(d => d.Bundle)
                    .WithMany(p => p.ItemBasket)
                    .HasForeignKey(d => d.BundleId)
                    .HasConstraintName("FK_ItemBasket_ItemBundle");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.ItemBasket)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemBasket_ShoppingCart");

                entity.HasOne(d => d.DeliverTo)
                    .WithMany(p => p.ItemBasket)
                    .HasForeignKey(d => d.DeliverToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemBasket_HidroAddress");

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.ItemBasket)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemBasket_Listing");

                entity.HasOne(d => d.Variation)
                    .WithMany(p => p.ItemBasket)
                    .HasForeignKey(d => d.VariationId)
                    .HasConstraintName("FK_ItemBasket_ItemVariation");
            });

            modelBuilder.Entity<ItemBundle>(entity =>
            {
                entity.Property(e => e.AvatarData).HasMaxLength(100);

                entity.Property(e => e.BundleName).HasMaxLength(30);

                entity.Property(e => e.BundlePrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemBundle)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemBundle_Item");
            });

            modelBuilder.Entity<ItemDetail>(entity =>
            {
                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.CarrierName).HasMaxLength(50);

                entity.Property(e => e.Certification).HasMaxLength(50);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.CompatibleWith).HasMaxLength(1);

                entity.Property(e => e.Condition).HasMaxLength(30);

                entity.Property(e => e.Dimensions).HasMaxLength(50);

                entity.Property(e => e.FormFactor).HasMaxLength(30);

                entity.Property(e => e.LockStatus).HasMaxLength(30);

                entity.Property(e => e.MadeIn).HasMaxLength(50);

                entity.Property(e => e.Materials).HasMaxLength(250);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.Packaging).HasMaxLength(30);

                entity.Property(e => e.Processing).HasMaxLength(1);

                entity.Property(e => e.ProductionNote).HasMaxLength(200);

                entity.Property(e => e.Quality).HasMaxLength(50);

                entity.Property(e => e.SerialOrSku).HasMaxLength(1);

                entity.Property(e => e.Size).HasMaxLength(30);

                entity.Property(e => e.ToBeUsedFor).HasMaxLength(1);

                entity.Property(e => e.Version).HasMaxLength(30);

                entity.Property(e => e.WarrantedBy).HasMaxLength(50);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemDetail)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemProperty_Item");
            });

            modelBuilder.Entity<ItemStock>(entity =>
            {
                entity.Property(e => e.StockQuantity)
                    .HasColumnType("decimal(10, 3)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Bundle)
                    .WithMany(p => p.ItemStock)
                    .HasForeignKey(d => d.BundleId)
                    .HasConstraintName("FK_ItemStock_ItemBundle");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemStock)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemStock_Item");

                entity.HasOne(d => d.Variation)
                    .WithMany(p => p.ItemStock)
                    .HasForeignKey(d => d.VariationId)
                    .HasConstraintName("FK_ItemStock_ItemVariation");
            });

            modelBuilder.Entity<ItemVariation>(entity =>
            {
                entity.Property(e => e.AvatarData).HasMaxLength(100);

                entity.Property(e => e.VariationName).HasMaxLength(30);

                entity.Property(e => e.VariationPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemVariation)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ItemVariation_Item");
            });

            modelBuilder.Entity<Listing>(entity =>
            {
                entity.Property(e => e.Caption).HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Headline).HasMaxLength(200);

                entity.Property(e => e.SellerNote).HasMaxLength(70);

                entity.Property(e => e.SellingFormat).HasDefaultValueSql("((0))");

                entity.Property(e => e.Title).HasMaxLength(80);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.MadeOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderTotal).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_ShoppingCart");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.AccountBalance)
                    .HasColumnType("decimal(18, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CardNumber).HasMaxLength(15);

                entity.Property(e => e.ExpiryDate).HasColumnType("date");

                entity.Property(e => e.HolderName).HasMaxLength(60);

                entity.Property(e => e.PaypalAddress).HasMaxLength(50);

                entity.Property(e => e.SecurityCode).HasMaxLength(5);

                entity.HasOne(d => d.Hidrogenian)
                    .WithMany(p => p.PaymentMethod)
                    .HasForeignKey(d => d.HidrogenianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentMethod_Hidrogenian");
            });

            modelBuilder.Entity<RawLocation>(entity =>
            {
                entity.Property(e => e.AlternateAddress).HasMaxLength(50);

                entity.Property(e => e.BuildingName).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(40);

                entity.Property(e => e.Commute).HasMaxLength(40);

                entity.Property(e => e.District).HasMaxLength(40);

                entity.Property(e => e.Group).HasMaxLength(30);

                entity.Property(e => e.Hamlet).HasMaxLength(40);

                entity.Property(e => e.Lane).HasMaxLength(10);

                entity.Property(e => e.PoBoxNumber).HasMaxLength(40);

                entity.Property(e => e.Postcode).HasMaxLength(10);

                entity.Property(e => e.Province).HasMaxLength(40);

                entity.Property(e => e.Quarter).HasMaxLength(40);

                entity.Property(e => e.State).HasMaxLength(40);

                entity.Property(e => e.StreetAddress).HasMaxLength(50);

                entity.Property(e => e.Suburb).HasMaxLength(50);

                entity.Property(e => e.Town).HasMaxLength(40);

                entity.Property(e => e.Ward).HasMaxLength(40);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.RawLocation)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RawLocation_Country");
            });

            modelBuilder.Entity<RoleClaimer>(entity =>
            {
                entity.Property(e => e.AllowTemporarily).HasMaxLength(255);

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

            modelBuilder.Entity<SellerRating>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(200);

                entity.Property(e => e.RatedOn).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.RatedBy)
                    .WithMany(p => p.SellerRatingRatedBy)
                    .HasForeignKey(d => d.RatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellerRating_HidroBuyer");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.SellerRatingSeller)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellerRating_HidroSeller");

                entity.HasOne(d => d.Trading)
                    .WithMany(p => p.SellerRating)
                    .HasForeignKey(d => d.TradingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellerRating_Listing");
            });

            modelBuilder.Entity<SellerReliability>(entity =>
            {
                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.SellerReliability)
                    .HasForeignKey(d => d.RatingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SellerReliability_SellerRating");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.Property(e => e.CartNote).HasMaxLength(100);

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.ShoppingCart)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShoppingCart_HidroTrader");
            });

            modelBuilder.Entity<StaffRole>(entity =>
            {
                entity.Property(e => e.WorkDescription).HasMaxLength(500);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.StaffRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StaffRole_StoreRole");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffRole)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StaffRole_StoreStaff");
            });

            modelBuilder.Entity<StoreOwner>(entity =>
            {
                entity.Property(e => e.JointOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OwnerNote).HasMaxLength(1000);

                entity.Property(e => e.SharedProfit).HasDefaultValueSql("((100))");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.StoreOwner)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreOwner_Hidrogenian");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreOwner)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreOwner_HidroStore");
            });

            modelBuilder.Entity<StoreRole>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.RoleName).HasMaxLength(30);
            });

            modelBuilder.Entity<StoreStaff>(entity =>
            {
                entity.Property(e => e.AddedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StoreStaff)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreStaff_Hidrogenian");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreStaff)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreStaff_HidroStore");
            });

            modelBuilder.Entity<Trading>(entity =>
            {
                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.TradingBuyer)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trading_HidroTrader_Buyer");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Trading)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trading_Order");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.TradingSeller)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trading_HidroTrader_Seller");
            });

            modelBuilder.Entity<TradingRating>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(200);

                entity.HasOne(d => d.Trading)
                    .WithMany(p => p.TradingRating)
                    .HasForeignKey(d => d.TradingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TradingRating_Trading");
            });

            modelBuilder.Entity<Warranty>(entity =>
            {
                entity.Property(e => e.Duration).HasDefaultValueSql("((12))");

                entity.Property(e => e.StartedOn).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Warranty)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Warranty_Item");
            });

            modelBuilder.Entity<WarrantyTerm>(entity =>
            {
                entity.Property(e => e.Content).HasMaxLength(2000);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Warranty)
                    .WithMany(p => p.WarrantyTerm)
                    .HasForeignKey(d => d.WarrantyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WarrantyTerm_Warranty");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
