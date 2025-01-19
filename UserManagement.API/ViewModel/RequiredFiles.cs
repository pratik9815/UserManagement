using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using UserManagement.Application.CustomValidation;

namespace UserManagement.Application.ViewModel;
public class RequiredFiles
{
    [DisplayName("First Field")]
    [ValidFields("Step3Field1 is required")]
    public string Step3Field1 { get; set; }

    [DisplayName("Second Field")]
    [ValidFields("Step3Field2 is required")]
    public string Step3Field2 { get; set; }
}
