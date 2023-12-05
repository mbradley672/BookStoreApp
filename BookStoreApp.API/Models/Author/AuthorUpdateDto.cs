using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Author;

public class AuthorUpdateDto : BaseDto
{
    [Required, StringLength(50)] public string FirstName { get; set; } = string.Empty;
    [Required, StringLength(50)] public string LastName { get; set; } = string.Empty;
    [Required, StringLength(250)] public string Bio { get; set; } = string.Empty;

}