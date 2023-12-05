using System.Linq.Expressions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repository;

public class AuthorRepository(BookStoreDbContext db) : Repository<Author>(db), IAuthorRepository
{
    public void Update(Author author)
    {
        db.Authors.Update(author);
    }

    public async Task<bool> AnyAsync(Expression<Func<Author, bool>> func)
    {
        return await DbSet.AnyAsync(func);
    }
}