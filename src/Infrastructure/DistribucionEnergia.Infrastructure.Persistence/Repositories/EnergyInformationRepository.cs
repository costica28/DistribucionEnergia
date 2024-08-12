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

        public async Task<List<HistoricalConsumptionDto>> GetHistoricalConsumptionBySegments(DateTime dateInitial, DateTime dateFinal)
        {

            var resultado = _dbContext.EnergyInformation
                            .Join(
                                _dbContext.Segments,
                                energia => energia.idTramo,
                                tramo => tramo.idTramo,
                                (energia, tramo) => new { Energia = energia, Tramo = tramo }
                            )
                            .Where(e => DateTime.Compare(e.Energia.fecha, dateInitial) >= 0 && DateTime.Compare(e.Energia.fecha, dateFinal) <= 0)
                            .GroupBy(e => new { e.Tramo.nombre })
                            .Select(g => new HistoricalConsumptionDto
                            {
                                tramo = g.Key.nombre,
                                consumo = g.Where(e => e.Energia.operacion.StartsWith("Consumo")).Sum(e => e.Energia.costo),
                                costos = g.Where(e => e.Energia.operacion.StartsWith("Costos")).Sum(e => e.Energia.costo),
                                perdidas = g.Where(e => e.Energia.operacion.StartsWith("Perdidas")).Sum(e => e.Energia.costo)
                            })
                            .OrderBy(r => r.tramo)
                            .ToList();
            return resultado;
        }

        public async Task<List<HistoricalByTypeClientDto>> GetHistoricalConsumptionByTypeClient(DateTime dateInitial, DateTime dateFinal)
        {
            var resultado = _dbContext.EnergyInformation
                            .Join(
                                _dbContext.Segments,
                                energia => energia.idTramo,
                                tramo => tramo.idTramo,
                                (energia, tramo) => new { Energia = energia, Tramo = tramo }
                            )
                            .Join(
                                _dbContext.Sectors,
                                energia => energia.Energia.idSector,
                                sector => sector.idSector,
                                (energia, sector)=> new
                                {
                                    Fecha = energia.Energia.fecha,
                                    Tramo = energia.Tramo.nombre,
                                    Operacion = energia.Energia.operacion,
                                    Costo = energia.Energia.costo,
                                    TipoCliente = sector.nombre
                                }
                            )
                            .Where(e => DateTime.Compare(e.Fecha, dateInitial) >= 0 && DateTime.Compare(e.Fecha, dateFinal) <= 0)
                            .GroupBy(e => new { e.Tramo, e.TipoCliente })
                            .Select(g => new HistoricalByTypeClientDto
                            {
                                tramo = g.Key.Tramo,
                                tipoCliente = g.Key.TipoCliente,
                                consumo = g.Where(e => e.Operacion.StartsWith("Consumo")).Sum(e => e.Costo),
                                costos = g.Where(e => e.Operacion.StartsWith("Costos")).Sum(e => e.Costo),
                                perdidas = g.Where(e => e.Operacion.StartsWith("Perdidas")).Sum(e => e.Costo)
                            })
                            .OrderBy(r => r.tramo)
                            .ToList();
            return resultado;
        }

        public async Task<List<ResponseWorstCustomerSegmentsDto>> GetWorstSegments(DateTime dateInitial, DateTime dateFinal, int take)
        {
            var resultado = _dbContext.EnergyInformation
                             .Join(
                                 _dbContext.Segments,
                                 energia => energia.idTramo,
                                 tramo => tramo.idTramo,
                                 (energia, tramo) => new { Energia = energia, Tramo = tramo }
                             )
                             .Join(
                                 _dbContext.Sectors,
                                 energia => energia.Energia.idSector,
                                 sector => sector.idSector,
                                 (energia, sector) => new
                                 {
                                     Fecha = energia.Energia.fecha,
                                     Tramo = energia.Tramo.nombre,
                                     Operacion = energia.Energia.operacion,
                                     Costo = energia.Energia.costo,
                                     TipoCliente = sector.nombre
                                 }
                             )
                             .Where(e => DateTime.Compare(e.Fecha, dateInitial) >= 0 && DateTime.Compare(e.Fecha, dateFinal) <= 0)
                             .GroupBy(e => new { e.Tramo, e.TipoCliente })
                             .Select(g => new ResponseWorstCustomerSegmentsDto
                             {
                                 tramo = g.Key.Tramo,
                                 tipoCliente = g.Key.TipoCliente,
                                 perdidas = g.Where(e => e.Operacion.StartsWith("Perdidas")).Sum(e => e.Costo)
                             })
                             .OrderBy(r => r.perdidas).Take(take)
                             .ToList();
            return resultado;
        }

        public async Task AddRange(List<EnergyInformation> energyInformations)
        {
            _dbContext.EnergyInformation.AddRange(energyInformations);
            await _dbContext.SaveChangesAsync();
        }
    }
}
