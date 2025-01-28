using System.ComponentModel.DataAnnotations;
using UserManagement.Application.CustomValidation;
using UserManagement.Domain.Common;

namespace UserManagement.Application.ViewModel;

public class CreateUserViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Username is required")]
    [MinLength(4, ErrorMessage = "Username must be at least 4 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter valid email address")]
    public string Email { get; set; }

    [ValidPassword]
    public string Password { get; set; }
    
    public Gender Gender { get; set; }
    //public List<SelectListItem> Roles { get; set; }
    //public List<SelectListItem> Roles { get; set; }
}
