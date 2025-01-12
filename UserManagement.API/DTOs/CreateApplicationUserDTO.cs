using System.ComponentModel.DataAnnotations;
using UserManagement.Application.CustomValidation;

namespace UserManagement.Application.DTOs;

public class CreateApplicationUserDTO
{   
    [Required(ErrorMessage = "Username is required")]
    [MinLength(4, ErrorMessage = "Username must be at least 4 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter valid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [ValidPassword]
    public string Password { get; set; }
    [Required]
    [Display(Name = "Confirm Password")]
    [MinLength(8,ErrorMessage = "Password must be at least 8 characters")]
    [Compare("Password",ErrorMessage ="Passwords must match")]
    [ValidPassword]
    public string ConfirmPassword { get; set; } 
}

public class LoginRequest
{
    [Required(ErrorMessage ="Username required")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [ValidPassword]
    public string Password { get; set; }
}
public class AuthResponse
{
    public string Token { get; set; }
    public string Username { get; set; }
    public List<string> Roles { get; set; }
    public DateTime Expiration { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }
}