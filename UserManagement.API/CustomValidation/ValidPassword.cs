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

    public void AddValidation(ClientModelValidationContext context)
    {
        //throw new NotImplementedException();
        context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-regex", ErrorMessage); 
    }

    public bool IsValidate(string password)
    {
        return IsValid(password);
    }
}
