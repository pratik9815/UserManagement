using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.CustomValidation;

public class ValidPassword : RegularExpressionAttribute, IClientModelValidator
{
    public ValidPassword() : base(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$")
    {
        //ErrorMessageResourceType = typeof(ValidationMessage);
        ErrorMessage = "Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one digit and one special character";
    }
    // Add the Required validation here
    public bool IsRequired { get; set; } = true;
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (IsRequired && value == null)
        {
            return new ValidationResult("Password is required.");
        }

        if (value != null && value is string password)
        {
            // Validate the password format using your regular expression
            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");

            if (!regex.IsMatch(password))
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-regex", ErrorMessage);
        context.Attributes.Add("data-val-regex-pattern", Pattern);
        if (IsRequired)
        {
            context.Attributes.Add("data-val-required", "Password is required.");
        }
    }

    public bool IsValidPassword(string password)
    {
        return IsValid(password);
    }
}
