using DistribucionEnergia.Core.Application.Contracts;
using DistribucionEnergia.Core.Domain.Models;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DistribucionEnergia.Core.Application.Features.LoadDataExcel
{
    public class LoadDataExcel
    {
        private IEnergyInformation _energyInformation;
        private ISegment _segment;
        private ISector _sector;

        public LoadDataExcel(IEnergyInformation energyInformation, ISegment segment, ISector sector)
        {
            _energyInformation = energyInformation;
            _segment = segment;
            _sector = sector;
        }

        public async Task<int> LoadData(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("Selecciona un archivo");

            if (!ValidExtensionExcel(file.FileName))
                throw new Exception("Carga un archivo valido de excel con las siguientes extensiones .xlsx");

            using (var memoryStream = new MemoryStream())
            {

                IWorkbook workbook = null;
                await file.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);


                // Detectar si es XLS o XLSX
                if (file.FileName.EndsWith(".xlsx"))
                {
                    workbook = new XSSFWorkbook(memoryStream); // Para archivos XLSX
                }
                else if (file.FileName.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(memoryStream); // Para archivos XLS
                }

                List<Segment> segments = await _segment.GetAll();
                List<Sector> sectors = await _sector.GetAll();

                if (segments.Count == 0)
                    throw new Exception("Deben de estar registrados los tramos permitidos en la base de datos");

                if (sectors.Count == 0)
                    throw new Exception("Deben de estar registrados los sectores permitidos en la base de datos");

                string[] TypeOperations = { "Consumo", "Costos", "Perdidas" };

                // Iterar sobre las hojas del archivo
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    List<Domain.Models.EnergyInformation> energyInformations = new List<Domain.Models.EnergyInformation>();
                    // Leer filas y celdas de la hoja
                    for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);
                        if (row != null)
                        {
                            Domain.Models.EnergyInformation energyInformation = new Domain.Models.EnergyInformation(); 

                            for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                            {
                                ICell cell = row.GetCell(cellIndex);
                                if (cell != null)
                                {
                                    if(cellIndex == 2)
                                    {
                                        energyInformation.idSector = sectors.FirstOrDefault(x => x.nombre == "Residencial").idSector;
                                    }
                                    else if (cellIndex == 3)
                                    {
                                        energyInformation.idSector = sectors.FirstOrDefault(x => x.nombre == "Comercial").idSector;
                                    }
                                    else if(cellIndex == 4)
                                    {
                                        energyInformation.idSector = sectors.FirstOrDefault(x => x.nombre == "Industrial").idSector;
                                    }

                                    switch (cell.CellType)
                                    {
                                        case CellType.String:
                                            var segment = segments.FirstOrDefault(x => x.nombre == cell.StringCellValue);
                                            if (cellIndex == 0 && segment != null)
                                            {
                                                energyInformation.idTramo = segment.idTramo;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(cell))
                                            {
                                                energyInformation.fecha = (DateTime)cell.DateCellValue;
                                            }
                                            else
                                                energyInformation.costo = cell.NumericCellValue;
                                            break;
                                        default:
                                            Console.WriteLine("Tipo de celda no manejado.");
                                            break;
                                    }
                                    if (cellIndex >= 2)
                                    {
                                        energyInformation.operacion = TypeOperations.FirstOrDefault(x => sheet.SheetName.ToLower().Contains(x.ToLower()));

                                        if (energyInformations.Count > 0 && cellIndex > 2)
                                        {
                                            var endRecord = energyInformations.LastOrDefault();
                                            energyInformation.idTramo = endRecord.idTramo;
                                            energyInformation.fecha = endRecord.fecha;
                                            energyInformations.Add(energyInformation);
                                        }
                                        else
                                            energyInformations.Add(energyInformation);
                                        energyInformation = new Domain.Models.EnergyInformation();
                                    }
                                }
                            }
                           
                            
                        }
                    }
                    await _energyInformation.AddRange(energyInformations);
                }
            }

            return 1;
        }

        private bool ValidExtensionExcel(string path)
        {
            string[] extensiones = { ".xlsx" };
            string fileExtension = Path.GetExtension(path).ToLower();
            return Array.Exists(extensiones, ext => ext.Equals(fileExtension));
        }

    }
}
