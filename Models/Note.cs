using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoteTaking.Models;

[Table("Notes", Schema = "notetaking")]
public partial class Note
{
    [Key]
    public decimal Id { get; set; }

    [Column(TypeName = "character varying")]
    public string? Title { get; set; }

    [Column(TypeName = "character varying")]
    public string? Description { get; set; }

    public decimal UserId { get; set; }

    public DateOnly? DeletedAt { get; set; }

    public decimal IsDeleted { get; set; }

    public DateOnly CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notes")]
    public virtual User User { get; set; } = null!;
}
