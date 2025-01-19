using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using UserManagement.Application.CustomValidation;

namespace UserManagement.Application.ViewModel;
public class PersonalDetails
{
    [DisplayName("First Field")]
    [ValidFields("Step1Field1 is required")]
    public string Step1Field1 { get; set; }

    [DisplayName("Second Field")]
    [ValidFields("Step1Field2 is required")]
    public string Step1Field2 { get; set; }
}
