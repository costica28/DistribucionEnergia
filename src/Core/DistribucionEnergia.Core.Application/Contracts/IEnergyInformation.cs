using DistribucionEnergia.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribucionEnergia.Core.Application.Contracts
{
    public interface IEnergyInformation
    {
        Task AddRange(List<EnergyInformation> energyInformations);
        Task<List<EnergyInformation>> WorstSegments(int take);
    }
}
