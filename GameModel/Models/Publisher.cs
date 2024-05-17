using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace GameModel.Models;

[Table("Publishers")]
[Index(nameof(PublisherName))]
public class Publisher
{
    #region Properties
    [Key]
    [Required]
    public int PublisherId { get; set; }

    public required string PublisherName { get; set; } = null!;

    public int PublisherYear { get; set; }

    [InverseProperty("Publisher")]
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    #endregion
}
