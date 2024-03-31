using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameModel;

[Table("Publisher")]
public partial class Publisher
{
    [Key]
    [Column("PublisherID")]
    public int PublisherID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string PublisherName { get; set; } = null!;

    [InverseProperty("Publisher")]
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}