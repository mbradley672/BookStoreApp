using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.User;

public class UserRegisterDto : UserLoginDto
{
    [Required, StringLength(25)]
    public string FirstName { get; set; }
    [Required, StringLength(25)]
    public string LastName { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
}