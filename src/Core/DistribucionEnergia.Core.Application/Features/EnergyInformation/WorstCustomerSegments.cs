using DistribucionEnergia.Commons;
using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Domain.Dto;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System;

namespace DistribucionEnergia.Core.Application.Features.EnergyInformation
{
    public class WorstCustomerSegments
    {
        private IEnergyInformation _energyInformation;

        public WorstCustomerSegments(IEnergyInformation energyInformation)
        {
            _energyInformation = energyInformation;
        }

        public async Task<List<ResponseWorstCustomerSegmentsDto>> GetTop20WorstCustomerSegments(string dateInitial, string dateFinal)
        {
            bool isFormatValidDateInitial = FormatDateValid.isFormatDateValid(dateInitial);
            bool isFormatValidDateFinal = FormatDateValid.isFormatDateValid(dateFinal);

            if (!isFormatValidDateInitial || !isFormatValidDateFinal)
                throw new Exception("La fecha debe estar en el formato yyyyy-MM-dd");

            DateTime dateInitialConvert = DateTime.ParseExact(dateInitial, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime dateFinalConvert = DateTime.ParseExact(dateFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            return await _energyInformation.GetWorstSegments(dateInitialConvert, dateFinalConvert, 20);
        }
    }
}
