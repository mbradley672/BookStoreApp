using BookStoreApp.API.Data;
using BookStoreApp.API.Repository.Interfaces;

namespace BookStoreApp.API.Repository;

public class BookRepository(BookStoreDbContext db) : Repository<Book>(db), IBookRepository
{
    public void Update(Book book)
    {
        db.Books.Update(book);
    }
}