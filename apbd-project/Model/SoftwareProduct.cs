using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_project.Model;

[Table("softwareProducts")]
public class SoftwareProduct
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = null!;
    [Required]
    public string Version { get; set; } = null!;
    [Required]
    [MaxLength(20)]
    public string Category { get; set; } = null!; // e.g. Finance, Education, ...
    [Required]
    public string Type { get; set; } = null!; // Subscription, One-time purchase, ...
}