using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookCatalogRestAPI.Models;
using BookCatalogRestAPI.Services;
using Xunit;

namespace BookCatalogRestAPI.Tests
{
    public class BookServiceTests
    {
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookService = new BookService();
        }

        [Theory]
        [InlineData("Test Book 1", "Test Author 1", 2021)]
        [InlineData("Test Book 2", "Test Author 2", 2022)]
        public void AddBook_ShouldAddBook(string title, string author, int year)
        {
            var book = new Book { Title = title, Author = author, Year = year };
            _bookService.AddBook(book);

            var result = _bookService.GetAllBooks();

            Assert.Contains(result, b => b.Title == title && b.Author == author && b.Year == year);
        }

        [Fact]
        public void AddBook_ShouldAssignUniqueIds()
        {
            var book1 = new Book { Title = "Test Book 1", Author = "Test Author 1", Year = 2021 };
            var book2 = new Book { Title = "Test Book 2", Author = "Test Author 2", Year = 2022 };
            _bookService.AddBook(book1);
            _bookService.AddBook(book2);

            var result = _bookService.GetAllBooks().ToList();
            Assert.Equal(2, result.Count);
            Assert.NotEqual(result[0].Id, result[1].Id);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnAllBooks()
        {
            var book1 = new Book { Title = "Test Book 1", Author = "Test Author 1", Year = 2021 };
            var book2 = new Book { Title = "Test Book 2", Author = "Test Author 2", Year = 2022 };
            _bookService.AddBook(book1);
            _bookService.AddBook(book2);

            var result = _bookService.GetAllBooks();
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData("Test Book 1", "Test Author 1", 2021)]
        [InlineData("Test Book 2", "Test Author 2", 2022)]
        public void GetBookById_ShouldReturnCorrectBook(string title, string author, int year)
        {
            var book = new Book { Title = title, Author = author, Year = year };
            _bookService.AddBook(book);

            var addedBook = _bookService.GetAllBooks().FirstOrDefault(b => b.Title == title && b.Author == author && b.Year == year);
            
            Assert.NotNull(addedBook);

            var result = _bookService.GetBookById(addedBook.Id);

            Assert.NotNull(result);
            Assert.Equal(title, result.Title);
            Assert.Equal(author, result.Author);
            Assert.Equal(year, result.Year);
        }

        [Theory]
        [InlineData(999)]
        public void GetBookById_ShouldReturnNullIfBookNotFound(int id)
        {
            Assert.Null(_bookService.GetBookById(id));
        }

        [Theory]
        [InlineData("Test Book 1", "Updated Test Book 1", "Updated Test Author 1", 2021)]
        [InlineData("Test Book 2", "Updated Test Book 2", "Updated Test Author 2", 2022)]
        public void UpdateBook_ShouldUpdateBook(string initialTitle, string updatedTitle, string updatedAuthor, int updatedYear)
        {
            var book = new Book { Title = initialTitle, Author = "Test Author", Year = 2021 };
            _bookService.AddBook(book);

            var addedBook = _bookService.GetAllBooks().FirstOrDefault(b => b.Title == initialTitle);
            
            Assert.NotNull(addedBook);

            addedBook.Title = updatedTitle;
            addedBook.Author = updatedAuthor;
            addedBook.Year = updatedYear;

            _bookService.UpdateBook(addedBook);

            var result = _bookService.GetBookById(addedBook.Id);

            Assert.Equal(updatedTitle, result.Title);
            Assert.Equal(updatedAuthor, result.Author);
            Assert.Equal(updatedYear, result.Year);
        }

        [Theory]
        [InlineData("Test Book 1", "Test Author 1", 2021)]
        [InlineData("Test Book 2", "Test Author 2", 2022)]
        public void DeleteBook_ShouldRemoveBook(string title, string author, int year)
        {
            var book = new Book { Title = title, Author = author, Year = year };
            _bookService.AddBook(book);

            var addedBook = _bookService.GetAllBooks().FirstOrDefault(b => b.Title == title && b.Author == author && b.Year == year);
            
            Assert.NotNull(addedBook);

            _bookService.DeleteBook(addedBook.Id);

            Assert.Null(_bookService.GetBookById(addedBook.Id));
        }
    }
}
