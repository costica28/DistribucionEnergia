using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Domain.Models;
using DistribucionEnergia.Infrastructure.Persistence.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribucionEnergia.Infrastructure.Persistence.Repositories
{
    public class EnergyInformationRepository : IEnergyInformation
    {
        protected DistribucionEnergiaDbContext _dbContext;

        public EnergyInformationRepository(DistribucionEnergiaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<EnergyInformation>> WorstSegments(int take)
        {
            throw new NotImplementedException();
        }

        public async Task AddRange(List<EnergyInformation> energyInformations)
        {
            _dbContext.EnergyInformation.AddRange(energyInformations);
            await _dbContext.SaveChangesAsync();
        }
    }
}
