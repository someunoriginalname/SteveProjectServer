using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameModel;

[Table("Game")]
public partial class Game
{
    [Key]
    [Column("AppID")]
    public int AppID { get; set; }

    [Column("Players")]
    public int Players { get; set; }

    [Column("Year")]
    public int Year { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Developer { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string GameName { get; set; } = null!;

    [ForeignKey("PublisherID")]
    public int PublisherID { get; set; }
    [InverseProperty("Games")]
    public virtual Publisher Publisher { get; set; } = null!;

}