using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book;

public class BookUpdateDto: BaseDto
{
    [Required, StringLength(50)] public string Title { get; set; } = string.Empty;
    [Required] public int AuthorId { get; set; }
    [Required, Range(1000, 3000)] public int Year { get; set; }
    [Required, Range(0, int.MaxValue)] public decimal Price { get; set; }
    public string Image { get; set; }
    [Required, StringLength(250)] public string Summary { get; set; } = string.Empty;
    [Required]public string Isbn { get; set; }
}