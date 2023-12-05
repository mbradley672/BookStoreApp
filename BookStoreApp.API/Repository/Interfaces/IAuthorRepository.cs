using System.Linq.Expressions;
using BookStoreApp.API.Data;

namespace BookStoreApp.API.Repository.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
    public void Update(Author author);
    Task<bool> AnyAsync(Expression<Func<Author, bool>> func);
}