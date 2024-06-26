using System.ComponentModel.DataAnnotations;

namespace apbd_project.Model;

// Discounts can only be applied when:
// -> the activity of the software contract
// -> at the time of subscription purchase

public class Discount
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string Offer { get; set; } = null!;
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
}