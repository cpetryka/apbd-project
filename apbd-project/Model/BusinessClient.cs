using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_project.Model;

[Table("business_clients")]
public class BusinessClient
{
    [Key]
    public int ClientId { get; set; }

    [Required]
    public string CompanyName { get; set; } = string.Empty;
    [Required]
    [MinLength(10)]
    [MaxLength(10)]
    public string KrsNumber { get; set; } = string.Empty; // Can't be changed after creation

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; } = null!;
}