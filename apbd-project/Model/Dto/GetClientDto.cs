namespace apbd_project.Model.Dto;

public class GetClientDto
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;

    // For individual clients
    public string PhoneNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Pesel { get; set; } = null!;

    // For business clients
    public string CompanyName { get; set; } = null!;
    public string KrsNumber { get; set; } = null!;
}