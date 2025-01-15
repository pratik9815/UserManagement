using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Domain.Entities;
public class UserFormModel
{
    [DisplayName("First Field")]
    [Required(ErrorMessage = "Field 1 is required.")]
    public string Step1Field1 { get; set; }

    [DisplayName("Second Field")]
    [Required(ErrorMessage = "Field 2 is required.")]
    public string Step1Field2 { get; set; }

    [DisplayName("First Field")]
    [Required(ErrorMessage = "Field 3 is required.")]
    public string Step2Field1 { get; set; }

    [DisplayName("Second Field")]
    [Required(ErrorMessage = "Field 4 is required.")]
    public string Step2Field2 { get; set; }

    [DisplayName("First Field")]
    [Required(ErrorMessage = "Field 5 is required.")]
    public string Step3Field1 { get; set; }

    [DisplayName("Second Field")]
    [Required(ErrorMessage = "Field 6 is required.")]
    public string Step3Field2 { get; set; }
}
