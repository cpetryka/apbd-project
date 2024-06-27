namespace apbd_project.Model.Dto;

public class AddOneTimePurchaseContractDto
{
    public int ClientId { get; set; }
    public int SoftwareProductId { get; set; }
    public string Version { set; get; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime SupportEndDate { get; set; }
}