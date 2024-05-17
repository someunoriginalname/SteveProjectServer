using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameModel.Models;

[Table("Games")]
[Index(nameof(Developer))]
[Index(nameof(Players))]
[Index(nameof(Year))]
[Index(nameof(GameName))]
[Index(nameof(Price))]
[Index(nameof(Revenue))]


public class Game
{
    #region Properties
    [Key]
    [Required]
    public int AppId { get; set; }

    [Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }

    public int Players { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Revenue { get; set; }

    public int Year { get; set; }

    public required string Developer { get; set; } = null!;

    public required string GameName { get; set; } = null!;

    [ForeignKey("PublisherId")]
    public int PublisherId { get; set; }
    #endregion

    #region Navigation Properties
    [InverseProperty("Games")]
    public virtual Publisher Publisher { get; set; } = null!;
    #endregion
}
