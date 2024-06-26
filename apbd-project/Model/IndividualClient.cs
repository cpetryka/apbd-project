using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_project.Model;

[Table("individual_clients")]
public class IndividualClient
{
    [Key]
    public int ClientId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Surname { get; set; } = string.Empty;
    [Required]
    [MinLength(11)]
    [MaxLength(11)]
    public string Pesel { get; set; } = string.Empty; // Can't be changed after creation
    public bool IsActive { get; set; } = true; // For soft delete

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; } = null!;
}