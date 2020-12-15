using CityGasWebApi.Models.BaseInfo;
using CityGasWebApi.Models.Common;
using CityGasWebApi.Models.Work;
using Microsoft.EntityFrameworkCore;

namespace CityGasWebApi.Models
{
    public class RuiJinChengWebContext : DbContext
    {
        public RuiJinChengWebContext(DbContextOptions<RuiJinChengWebContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<OilStation> OilStation { get; set; }
        public DbSet<Truck> Truck { get; set; }

        public DbSet<WorkTicket> WorkTicket { get; set; }

        public DbSet<Files> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

