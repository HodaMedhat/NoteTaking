using NoteTaking.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTaking.ViewModel
{
    public class NoteVM
    {
        public decimal Id { get; set; }

        public string? Title { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public decimal UserId { get; set; }

        public DateOnly? DeletedAt { get; set; }

        public decimal IsDeleted { get; set; }

        public DateOnly CreatedAt { get; set; } = new DateOnly();
        public virtual User User { get; set; } = null!;
    }
}
