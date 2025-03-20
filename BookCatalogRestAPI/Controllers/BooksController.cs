using Microsoft.AspNetCore.Mvc;
using BookCatalogRestAPI.Models;
using BookCatalogRestAPI.Services;

namespace BookCatalogRestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        // https://localhost:7298/api/books/get-all-books to get details of all books.
        [HttpGet]
        [Route("get-all-books")]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            _logger.LogInformation("Getting all books");

            var books = _bookService.GetAllBooks();

            return Ok(books);
        }

        // https://localhost:7298/api/books/book/{id} to get details of a specific book by ID.
        [HttpGet]
        [Route("book/{id:int}")]
        public ActionResult<Book> GetBook(int id)
        {
            _logger.LogInformation("Getting book with id {Id}", id);

            var book = _bookService.GetBookById(id);

            if (book == null)
            {
                _logger.LogWarning("Book with id {Id} not found", id);
                return NotFound(new { Message = $"Book with id {id} not found" });
            }

            return Ok(book);
        }

        // https://localhost:7298/api/books/add to add a new book.
        [HttpPost]
        [Route("add")]
        public ActionResult AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid book model");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Adding a new book");

            _bookService.AddBook(book);
            
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, new { Message = "Book added successfully", book });
        }

        // https://localhost:7298/api/books/update/{id} to update a book by ID.
        [HttpPut]
        [Route("update/{id:int}")]
        public ActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                _logger.LogWarning("Book id mismatch");
                return BadRequest(new { Message = "Book id mismatch" });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid book model");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating book with id {Id}", id);
            
            _bookService.UpdateBook(book);

            return Ok(new { Message = "Book updated successfully", book });
        }

        // https://localhost:7298/api/books/delete/{id} to delete a book by ID.
        [HttpDelete]
        [Route("delete/{id:int}")]
        public ActionResult DeleteBook(int id)
        {
            _logger.LogInformation("Deleting book with id {Id}", id);

            var book = _bookService.GetBookById(id);
            
            if (book == null)
            {
                _logger.LogWarning("Book with id {Id} not found", id);
                return NotFound(new { Message = $"Book with id {id} not found" });
            }

            _bookService.DeleteBook(id);

            return Ok(new { Message = "Book deleted successfully" });
        }
    }
}
