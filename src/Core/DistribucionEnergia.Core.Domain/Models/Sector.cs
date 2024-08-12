using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistribucionEnergia.Core.Domain.Models
{
    [Table("Sector")]
    public class Sector
    {
        [Key]
        public int idSector { get; set; }
        public string nombre { get; set; } = string.Empty;
    }
}
