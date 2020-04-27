using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WaterLibrary.DbContexts {

    public partial class WaterDbContext {
        
        private IConfiguration Configuration { get; }

        public WaterDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseMySql(Configuration.GetConnectionString("PhotoServer"), x => x.ServerVersion("10.4.11-mariadb"));
            }
        }
    }
}