namespace apbd_project.Model.Dto;

public class AddIndividualClientDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Pesel { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}