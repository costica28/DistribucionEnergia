using System.Collections.Generic;

namespace DistribucionEnergia.Core.Domain.Dto
{
    public class ResponseHistoricalByTypeClienteDto
    {
        public string tipoCliente { get; set; } = string.Empty;
        public List<HistoricalConsumptionDto> tramos { get; set; }
    }
}
