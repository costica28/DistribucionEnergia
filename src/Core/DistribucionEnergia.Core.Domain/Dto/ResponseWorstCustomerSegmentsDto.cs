namespace DistribucionEnergia.Core.Domain.Dto
{
    public class ResponseWorstCustomerSegmentsDto
    {
        public string tramo { get; set; } = string.Empty;
        public string tipoCliente { get; set; } = string.Empty;
        public double perdidas { get; set; }
    }
}
