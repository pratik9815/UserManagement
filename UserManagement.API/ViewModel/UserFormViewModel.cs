namespace UserManagement.Application.ViewModel;

public class UserFormViewModel
{
    public PersonalDetails PersonalDetails { get; set; }
    public FamilyDetails FamilyDetails { get; set; }
    public RequiredFiles RequiredFiles { get; set; }
    public UserFormViewModel()
    {
        PersonalDetails = new PersonalDetails();
    }
}
