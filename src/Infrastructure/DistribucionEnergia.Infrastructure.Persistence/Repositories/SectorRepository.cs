using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Domain.Models;
using DistribucionEnergia.Infrastructure.Persistence.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribucionEnergia.Infrastructure.Persistence.Repositories
{
    public class SectorRepository : ISector
    {
        protected DistribucionEnergiaDbContext _dbContext;

        public SectorRepository(DistribucionEnergiaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Sector>> GetAll()
        {
            return await _dbContext.Sectors.ToListAsync();
        }
    }
}
