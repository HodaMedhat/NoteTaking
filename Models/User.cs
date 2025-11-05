using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NoteTaking.Models;

[Table("Users", Schema = "notetaking")]
public partial class User
{
    [Key]
    public decimal Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string UserName { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string Password { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string Email { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
