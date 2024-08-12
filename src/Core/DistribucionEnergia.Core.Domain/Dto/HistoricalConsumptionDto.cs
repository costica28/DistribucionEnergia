using System;

namespace DistribucionEnergia.Core.Domain.Dto
{
    public class HistoricalConsumptionDto
    {
        public string tramo { get; set; } = string.Empty;
        public double consumo { get; set; }
        public double costos { get; set; }
        public double perdidas { get; set; }
    }
}
