using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_project.Model;

[Table(("payments"))]
public class Payment
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int ContractId { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public DateTime PaymentDate { get; set; }

    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; } = null!;
}