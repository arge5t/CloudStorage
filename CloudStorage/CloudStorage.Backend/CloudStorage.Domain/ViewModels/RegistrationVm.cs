using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CloudStorage.Domain.ViewModels
{
    public class RegistrationVm
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
