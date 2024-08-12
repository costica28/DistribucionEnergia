using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Domain.Dto;
using DistribucionEnergia.Core.Domain.Models;
using DistribucionEnergia.Infrastructure.Persistence.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<EnergyInformation>> GetByRangeDate(DateTime dateInitial, DateTime dateFinal)
        {
            return await _dbContext.EnergyInformation.Where(x => DateTime.Compare(x.fecha, dateInitial) >= 0 && DateTime.Compare(x.fecha, dateFinal) <= 0).ToListAsync();
        }

        public async Task<List<HistoricalConsumptionBySegmentDto>> GetHistoricalConsumptionBySegments(DateTime dateInitial, DateTime dateFinal)
        {

            var resultado = _dbContext.EnergyInformation
                            .Join(
                                _dbContext.Segments,
                                energia => energia.idTramo,
                                tramo => tramo.idTramo,
                                (energia, tramo) => new { Energia = energia, Tramo = tramo }
                            )
                            .Where(e => DateTime.Compare(e.Energia.fecha, dateInitial) >= 0 && DateTime.Compare(e.Energia.fecha, dateFinal) <= 0)
                            .GroupBy(e => new { e.Tramo.nombre, e.Energia.fecha })
                            .Select(g => new HistoricalConsumptionBySegmentDto
                            {
                                fecha = g.Key.fecha.ToString(),
                                tramo = g.Key.nombre,
                                consumo = g.Where(e => e.Energia.operacion.StartsWith("Consumo")).Sum(e => e.Energia.costo),
                                costos = g.Where(e => e.Energia.operacion.StartsWith("Costos")).Sum(e => e.Energia.costo),
                                perdidas = g.Where(e => e.Energia.operacion.StartsWith("Perdidas")).Sum(e => e.Energia.costo)
                            })
                            .OrderBy(r => r.fecha)
                            .ToList();
            return resultado;
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
