using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'

namespace BookStoreApp.API.Controllers;

[ApiController, Route("api/[controller]")]
public class AuthorController(IAuthorRepository authorRepository, IMapper mapper, ILogger<AuthorController> logger) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<ICollection<AuthorReadOnlyDto>>> GetAuthors()
    {
        return Ok(mapper.Map<ICollection<AuthorReadOnlyDto>>(await authorRepository.GetAll()));
    }

    [HttpPost, Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateAuthor(AuthorCreateDto dto)
    {
        var author = mapper.Map<Author>(dto);
        await authorRepository.AddAsync(author);
        var result = await authorRepository.SaveChangesAsync();
        if (result > 0)
        {
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }
        logger.LogError($"Error creating author from DTO {dto}");
        return BadRequest();
    }

    [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateAuthor(int id, AuthorUpdateDto authorDto)
    {
        try
        {
            var author = await authorRepository.GetSingleOrDefault(a => a.Id == id);
            if (author == null && author?.Id == id)
            {
                logger.LogWarning($"Author with id {id} not found");
                return NotFound();
            }

            mapper.Map(authorDto, author);
            await authorRepository.SetEntityState(author!, EntityState.Modified);

            authorRepository.Update(author!);
            var result = await authorRepository.SaveChangesAsync();
            if (result == 0)
            {
                logger.LogError($"Error updating author with id {id} from DTO {authorDto}");
                return BadRequest();
            }
            return Ok(author);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,$"Problem updating author with id {id} from DTO {authorDto}");
            throw;
        }
    }

    [HttpDelete("{id:int}"), Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var existingAuthor = await authorRepository.GetSingleOrDefault(a => a.Id == id);
        if (existingAuthor == null)
        {
            logger.LogWarning($"Author with id {id} not found");
            return NotFound();
        }
        authorRepository.Remove(existingAuthor);
        await authorRepository.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
    {
        try
        {
            var author = await authorRepository.GetSingleOrDefault(a => a.Id == id);
            if (author == null)
            {
                logger.LogWarning($"Author with id {id} not found");
                return NotFound();
            }
            return Ok(mapper.Map<AuthorReadOnlyDto>(author));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting author");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id:int}/books")]
    public async Task<IActionResult> GetAuthorsBooks(int id)
    {
        var author = await authorRepository.GetSingleOrDefault(a => a.Id == id, "Books");
        if (author == null)
        {
            logger.LogWarning($"Author with id {id} not found");
            return NotFound();
        }
        return Ok(author.Books);
    }

    private async Task<bool> AuthorExists(int id)
    {
        return await authorRepository.AnyAsync(a => a.Id == id) != null;
    }
}