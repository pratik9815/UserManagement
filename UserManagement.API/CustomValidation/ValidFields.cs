using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.CustomValidation;
public class ValidFields : ValidationAttribute, IClientModelValidator
{
    public ValidFields(string message)
    {
        ErrorMessage = message;
    }
    public void AddValidation(ClientModelValidationContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        // Add a "data-val" attribute to indicate client-side validation
        context.Attributes["data-val"] = "true";

        // Add a custom error message for client-side validation
        context.Attributes["data-val-required"] = ErrorMessage;
    }
    public override bool IsValid(object? value)
    {
        // Ensure the value is not null or an empty string
        if (value == null)
            return false;

        if (value is string strValue && string.IsNullOrWhiteSpace(strValue))
            return false;

        return true;
    }
}
