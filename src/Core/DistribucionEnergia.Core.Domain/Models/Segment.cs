using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistribucionEnergia.Core.Domain.Models
{
    [Table("Tramo")]
    public class Segment
    {
        [Key]
        public int idTramo { get; set; }
        public string nombre { get; set; } = string.Empty;
    }
}
