using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Application.Features.LoadDataExcel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Seleccione un archivo" });

            LoadDataExcel loadDataExcel = new LoadDataExcel(_energyInformation, _segment, _sector);
            return Ok(await loadDataExcel.LoadData(file));
        }
    }
}
