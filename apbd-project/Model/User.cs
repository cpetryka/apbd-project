using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_project.Model;

[Table("users")]
public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(20)]
    public string Username { get; set; }
    [Required]
    public string HashedPassword { get; set; }
    [Required]
    [MaxLength(20)]
    public string Role { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}