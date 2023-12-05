using BookStoreApp.API.Data;

namespace BookStoreApp.API.Repository.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    public void Update(Book book);
}