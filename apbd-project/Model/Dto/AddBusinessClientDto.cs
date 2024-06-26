namespace apbd_project.Model.Dto;

public class AddBusinessClientDto
{
    public string CompanyName { get; set; } = null!;
    public string KrsNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}