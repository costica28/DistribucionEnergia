using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Domain.Models;
using DistribucionEnergia.Infrastructure.Persistence.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribucionEnergia.Infrastructure.Persistence.Repositories
{

    public class SegmentRepository : ISegment
    {
        protected DistribucionEnergiaDbContext _dbContext;
        public SegmentRepository(DistribucionEnergiaDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<List<Segment>> GetAll()
        {
            return await _dbContext.Segments.ToListAsync();
        }
    }
}
