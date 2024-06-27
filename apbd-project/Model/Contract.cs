using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_project.Model;

[Table("contracts")]
public class Contract
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public int SoftwareProductId { get; set; }
    [Required]
    public decimal Price { get; set; } // Includes all discounts
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime SignedDate { get; set; } // If null, it means that the contract is not signed yet

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; } = null!;
    [ForeignKey(nameof(SoftwareProductId))]
    public SoftwareProduct SoftwareProduct { get; set; } = null!;

    public OneTimePurchase? OneTimePurchase { get; set; } = null!;
}