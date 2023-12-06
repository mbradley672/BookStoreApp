using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers;


[ApiController, Route("api/[controller]")]
public class BooksController(BookStoreDbContext db, ILogger<BooksController> logger, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
    {
        var books = await db.Books
            .Include(x => x.Author)
            .ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        
        
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
    {
        var book = await db.Books
            .ProjectTo<BookDetailsDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == id);
            
        if (book == null)
        {
            logger.LogError($"Book with {id} is nto found in the Db");
            return NotFound();
        }

        
        return Ok(book);
    }
    
    [HttpPost, Authorize(Roles = "Administrator")]
    public async Task<ActionResult<BookReadOnlyDto>> CreateBook(BookCreateDto bookDto)
    {
        var book = mapper.Map<Book>(bookDto);
        db.Books.Add(book);
        await db.SaveChangesAsync();
        var createdBookDto = mapper.Map<BookReadOnlyDto>(book);
        return CreatedAtAction(nameof(GetBook), new { id = createdBookDto.Id }, createdBookDto);
    }
    
    [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateBook(int id, BookUpdateDto bookDto)
    {
        var book = await db.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        mapper.Map(bookDto, book);
        db.Books.Update(book);
        await db.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id:int}"), Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await db.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> SearchBooks(string searchTerm)
    {
        var books = await db.Books.Where(x => x.Title.Contains(searchTerm) || x.Summary.Contains(searchTerm))
            .ProjectTo<BookDetailsDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        return Ok(books);
    }
    
    [HttpGet("author/{authorId:int}")]
    public async Task<ActionResult<IEnumerable<BookDetailsDto>>> GetBooksByAuthor(int authorId)
    {
        var books = await db.Books.Where(x => x.AuthorId == authorId)
            .ProjectTo<BookDetailsDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        return Ok(books);
    }
}