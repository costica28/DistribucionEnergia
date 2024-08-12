using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistribucionEnergia.Core.Domain.Models
{
    [Table("InformacionEnergia")]
    public class EnergyInformation
    {
        [Key]
        public int idInformacionEnergia { get; set; }
        public int idTramo { get; set; }
        public int idSector { get; set; }
        public double costo { get; set; }
        public DateTime fecha { get; set; }
        public string operacion { get; set; } = string.Empty;
    }
}
