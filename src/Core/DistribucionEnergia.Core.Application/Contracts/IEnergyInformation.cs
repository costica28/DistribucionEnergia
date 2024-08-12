using DistribucionEnergia.Core.Domain.Dto;
using DistribucionEnergia.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribucionEnergia.Core.Application.Contracts
{
    public interface IEnergyInformation
    {
        Task AddRange(List<EnergyInformation> energyInformations);
        Task<List<HistoricalConsumptionBySegmentDto>> GetHistoricalConsumptionBySegments(DateTime dateInitial, DateTime dateFinal);
        Task<List<EnergyInformation>> GetByRangeDate(DateTime dateInitial, DateTime dateFinal);
        Task<List<EnergyInformation>> WorstSegments(int take);
    }
}
