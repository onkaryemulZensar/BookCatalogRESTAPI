using BookCatalogRestAPI.Models;

namespace BookCatalogRestAPI.Services
{
    public class BookService : IBookService
    {
        private readonly List<Book> _books = new List<Book>();

        public IEnumerable<Book> GetAllBooks() => _books;

        public Book GetBookById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);

            return book;
        }

        public void AddBook(Book book)
        {
            book.Id = _books.Count + 1;

            _books.Add(book);
        }

        public void UpdateBook(Book book)
        {
            var existingBook = GetBookById(book.Id);

            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Year = book.Year;
            }
        }

        public void DeleteBook(int id)
        {
            var book = GetBookById(id);

            if(book != null)
            {
                _books.Remove(book);
            }
        }
    }
}

