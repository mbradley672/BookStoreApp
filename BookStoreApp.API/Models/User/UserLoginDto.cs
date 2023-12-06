using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.User;

public class UserLoginDto
{
    [Required]
    public string? UserName { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
}