using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using UserManagement.Application.CustomValidation;

namespace UserManagement.Application.ViewModel;
public class FamilyDetails
{
    [DisplayName("First Field")]
    [ValidFields("Step2Field1 is required")]
    public string Email { get; set; }

    [DisplayName("Second Field")]
    [ValidFields("Step2Field2 is required")]
    public string Phone { get; set; }
}
