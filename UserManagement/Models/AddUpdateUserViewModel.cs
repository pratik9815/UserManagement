using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using UserManagement.Application.CustomValidation;
using UserManagement.Domain.Common;

namespace UserManagement.Api.Models;

public class AddUpdateUserViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Username is required")]
    [MinLength(4, ErrorMessage = "Username must be at least 4 characters")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter valid email address")]
    public string Email { get; set; }
    //[ValidPassword]
    public string Password { get; set; }
    public Gender Gender { get; set; }
    public List<SelectListItem>? Roles { get; set; }
    public string RoleId { get; set; }
    //public string RoleName { get; set; }    
    public bool IsActive { get; set; }
}
