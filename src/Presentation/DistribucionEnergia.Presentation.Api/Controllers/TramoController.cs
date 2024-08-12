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

        /// <summary>
        /// Permite guardar la información del excel en la base de datos, metodo utilizado para cargar la data
        /// </summary>
        /// <param name="file">Archivo xlsx que se carga</param>
        /// <returns></returns>
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
        [ProducesResponseType(typeof(HistoricalConsumptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseExceptionDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HistoricalConsumptionBySegment(string dateInitial, string dateFinal)
        {
            try
            {
                var historicalConsumptionBySegment = new Core.Application.Features.EnergyInformation.HistoricalConsumptionBySegment(_energyInformation);
                return Ok(new { data = await historicalConsumptionBySegment.GetHistoricalConsumptionBySegment(dateInitial, dateFinal) });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseExceptionDto() { message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// Permite consultar el historico de consusmos por clientes
        /// </summary>
        /// <param name="dateInitial">Fecha inicial</param>
        /// <param name="dateFinal">Fecha final</param>
        /// <returns></returns>
        [HttpGet]
        [Route("HistoricalConsumptionByTypeClient/{dateInitial}/{dateFinal}")]
        [ProducesResponseType(typeof(ResponseHistoricalByTypeClienteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseExceptionDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HistoricalConsumptionByTypeClient(string dateInitial, string dateFinal)
        {
            try
            {
                var historicalConsumptionByTypeClient = new Core.Application.Features.EnergyInformation.HistoricalConsumptionByTypeClient(_energyInformation);
                return Ok(new { data = await historicalConsumptionByTypeClient.GetHistorical(dateInitial, dateFinal) });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseExceptionDto() { message = ex.Message.ToString() });
            }
        }

        /// <summary>
        /// Permite consultar los peores 20 tramos/clientes que generan perdidas
        /// </summary>
        /// <param name="dateInitial">Fecha inicial</param>
        /// <param name="dateFinal">Fecha final</param>
        /// <returns></returns>
        [HttpGet]
        [Route("WorstTop20CustomerSegments/{dateInitial}/{dateFinal}")]
        [ProducesResponseType(typeof(ResponseWorstCustomerSegmentsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseExceptionDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> WorstCustomerSegmentss(string dateInitial, string dateFinal)
        {
            try
            {
                var worstCustomerSegments = new Core.Application.Features.EnergyInformation.WorstCustomerSegments(_energyInformation);
                return Ok(new { data = await worstCustomerSegments.GetTop20WorstCustomerSegments(dateInitial, dateFinal) });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseExceptionDto() { message = ex.Message.ToString() });
            }
        }
    }
}
