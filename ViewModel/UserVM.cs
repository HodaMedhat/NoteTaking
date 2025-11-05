using NoteTaking.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTaking.ViewModel
{
    public class UserVM
    {
        public decimal Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(5, ErrorMessage = "PassWord has to be at least 5 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please Enter a vaild Email") ]
        public string Email { get; set; } = null!;

        public virtual ICollection<NoteVM> Notes { get; set; } = new List<NoteVM>();
    }
}
