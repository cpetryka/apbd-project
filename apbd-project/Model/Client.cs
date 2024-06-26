using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_project.Model;

[Table("clients")]
public class Client
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Address { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string PhoneNumber { get; set; } = null!;

    // TODO: should it be a collection or a single object?
    public IndividualClient IndividualClient { get; set; } = null!;
    public BusinessClient BusinessClient { get; set; } = null!;
}