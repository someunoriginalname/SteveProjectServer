
using System.ComponentModel.DataAnnotations.Schema;

namespace SteveProjectServer.DTO
{
    public class PublisherUnits
    {
        public required string Publisher { get; set; }
        public int PublisherID { get; set; }

        public int Units { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Sales { get; set; }

        //[Column(TypeName = "decimal(8,2)")]
        //public decimal Revenue { get; set; }
    }
}
