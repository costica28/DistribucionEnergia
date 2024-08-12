using DistribucionEnergia.Commons;
using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DistribucionEnergia.Core.Application.Features.EnergyInformation
{
    public class HistoricalConsumptionByTypeClient
    {
        private IEnergyInformation _energyInformation;

        public HistoricalConsumptionByTypeClient(IEnergyInformation energyInformation)
        {
            _energyInformation = energyInformation;
        }

        public async Task<List<ResponseHistoricalByTypeClienteDto>> GetHistorical(string dateInitial, string dateFinal)
        {
            bool isFormatValidDateInitial = FormatDateValid.isFormatDateValid(dateInitial);
            bool isFormatValidDateFinal = FormatDateValid.isFormatDateValid(dateFinal);

            if (!isFormatValidDateInitial || !isFormatValidDateFinal)
                throw new Exception("La fecha debe estar en el formato yyyyy-MM-dd");

            DateTime dateInitialConvert = DateTime.ParseExact(dateInitial, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime dateFinalConvert = DateTime.ParseExact(dateFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var historicalByTypeClients = await _energyInformation.GetHistoricalConsumptionByTypeClient(dateInitialConvert, dateFinalConvert);

            var typeClients = historicalByTypeClients.Select(x => x.tipoCliente).Distinct().ToList();

            List<ResponseHistoricalByTypeClienteDto> responseHistoricalByTypeClienteDtos = new List<ResponseHistoricalByTypeClienteDto>();
            foreach (var typeClient in typeClients)
            {
                ResponseHistoricalByTypeClienteDto responseHistoricalByTypeClienteDto = new ResponseHistoricalByTypeClienteDto();
                responseHistoricalByTypeClienteDto.tipoCliente = typeClient;
                var segments = historicalByTypeClients.Where(x => x.tipoCliente == typeClient).Select(t => new HistoricalConsumptionDto()
                {
                    tramo = t.tramo,
                    consumo = t.consumo,
                    costos = t.costos,
                    perdidas = t.perdidas
                }).ToList();
                responseHistoricalByTypeClienteDto.tramos = new List<HistoricalConsumptionDto>();
                responseHistoricalByTypeClienteDto.tramos.AddRange(segments);
                responseHistoricalByTypeClienteDtos.Add(responseHistoricalByTypeClienteDto);
            }

            return responseHistoricalByTypeClienteDtos;
        }
    }
}
