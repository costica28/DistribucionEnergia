using DistribucionEnergia.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DistribucionEnergia.Infrastructure.Persistence.Persistence
{
    public class DistribucionEnergiaDbContext: DbContext
    {
        public DistribucionEnergiaDbContext(DbContextOptions<DistribucionEnergiaDbContext> options) : base(options){}


        public DbSet<Segment> Segments { get; set; }
        public DbSet<EnergyInformation> EnergyInformation { get; set; }
        public DbSet<Sector> Sectors { get; set; }
    }
}
