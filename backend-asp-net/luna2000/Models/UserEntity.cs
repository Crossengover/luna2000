using System.ComponentModel.DataAnnotations;

namespace luna2000.Models;

public class UserEntity
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }
}
