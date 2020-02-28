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