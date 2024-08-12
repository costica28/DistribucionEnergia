using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Application.Features.LoadDataExcel;
using DistribucionEnergia.Core.Domain.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DistribucionEnergia.Presentation.Api.Controllers
{
    [Route("[controller]")]
    public class TramoController : ControllerBase
    {
        private IEnergyInformation _energyInformation;
        private ISegment _segment;
        private ISector _sector;

        public TramoController(IEnergyInformation energyInformation, ISegment segment, ISector sector)
        {
            _energyInformation = energyInformation;
            _segment = segment;
            _sector = sector;
        }

        [HttpPost]
        [Route("CargarInformacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CargarInformacion(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "Seleccione un archivo" });

                LoadDataExcel loadDataExcel = new LoadDataExcel(_energyInformation, _segment, _sector);
                return Ok(await loadDataExcel.LoadData(file));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Permite consultar el historico de consusmos por tramos
        /// </summary>
        /// <param name="dateInitial">Fecha inicial</param>
        /// <param name="dateFinal">Fecha final</param>
        /// <returns></returns>
        [HttpGet]
        [Route("HistoricalConsumptionBySegment/{dateInitial}/{dateFinal}")]
        [ProducesResponseType(typeof(HistoricalConsumptionBySegmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespuestaExceptionDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HistoricalConsumptionBySegment(string dateInitial, string dateFinal)
        {
            try
            {
                var historicalConsumptionBySegment = new Core.Application.Features.EnergyInformation.HistoricalConsumptionBySegment(_energyInformation, _segment);
                return Ok( new { data = await historicalConsumptionBySegment.GetHistoricalConsumptionBySegment(dateInitial, dateFinal) });
            }
            catch (Exception ex)
            {
                return BadRequest(new RespuestaExceptionDto(){ message = ex.Message.ToString() });
            }
        }
    }
}
