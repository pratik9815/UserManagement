using System.ComponentModel;
using UserManagement.Application.CustomValidation;

namespace UserManagement.Application.ViewModel;
public class PersonalDetails
{
    [DisplayName("First Field")]
    [ValidFields("Step1Field1 is required")]
    public string FirstName { get; set; }

    [DisplayName("Second Field")]
    [ValidFields("Step1Field2 is required")]
    public string LastName { get; set; }
}
