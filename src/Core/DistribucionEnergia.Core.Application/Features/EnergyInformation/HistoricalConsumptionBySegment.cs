﻿using DistribucionEnergia.Core.Application.Contracts;
using System.Globalization;
using System;
using System.Threading.Tasks;
using DistribucionEnergia.Commons;
using System.Collections.Generic;
using DistribucionEnergia.Core.Domain.Dto;
using System.Linq;

namespace DistribucionEnergia.Core.Application.Features.EnergyInformation
{
    public class HistoricalConsumptionBySegment
    {
        private IEnergyInformation _energyInformation;

        public HistoricalConsumptionBySegment(IEnergyInformation energyInformation)
        {
            _energyInformation = energyInformation;
        }

        public async Task<List<HistoricalConsumptionDto>> GetHistoricalConsumptionBySegment(string dateInitial, string dateFinal)
        {
            bool isFormatValidDateInitial = FormatDateValid.isFormatDateValid(dateInitial);
            bool isFormatValidDateFinal = FormatDateValid.isFormatDateValid(dateFinal);

            if (!isFormatValidDateInitial || !isFormatValidDateFinal)
                throw new Exception("La fecha debe estar en el formato yyyyy-MM-dd");

            DateTime dateInitialConvert = DateTime.ParseExact(dateInitial, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime dateFinalConvert = DateTime.ParseExact(dateFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            return await _energyInformation.GetHistoricalConsumptionBySegments(dateInitialConvert, dateFinalConvert);            
        }

    }
}
