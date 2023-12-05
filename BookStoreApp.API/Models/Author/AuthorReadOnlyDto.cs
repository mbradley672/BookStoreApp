namespace BookStoreApp.API.Models.Author;

public class AuthorReadOnlyDto : BaseDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

}