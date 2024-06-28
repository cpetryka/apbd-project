using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_project.Model;

[Table("oneTimePurchase")]
public class OneTimePurchase
{
    [Key]
    public int ContractId { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public string Version { get; set; } = string.Empty; // Client buys a specific version of the software
    [Required]
    public DateTime SupportEndDate { get; set; }

    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; } = null!;


}