namespace apbd_project.Model.Dto;

public class GetDiscountDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Offer { get; set; }
    public int Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}