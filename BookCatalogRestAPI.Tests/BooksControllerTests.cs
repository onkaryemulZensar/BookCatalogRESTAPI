using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookCatalogRestAPI.Controllers;
using BookCatalogRestAPI.Models;
using BookCatalogRestAPI.Services;
using Moq;
using Xunit;

namespace BookCatalogRestAPI.Tests
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly Mock<ILogger<BooksController>> _mockLogger;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _mockLogger = new Mock<ILogger<BooksController>>();
            _controller = new BooksController(_mockBookService.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetBooks_ShouldReturnAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Test Book 1", Author = "Test Author 1", Year = 2021 },
                new Book { Id = 2, Title = "Test Book 2", Author = "Test Author 2", Year = 2022 }
            };

            _mockBookService.Setup(service => service.GetAllBooks()).Returns(books);

            // Act
            var result = _controller.GetBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBooks = Assert.IsType<List<Book>>(okResult.Value);
            Assert.Equal(2, returnBooks.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetBook_ShouldReturnBook_WhenBookExists(int bookId)
        {
            // Arrange
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Test Author", Year = 2021 };
            _mockBookService.Setup(service => service.GetBookById(bookId)).Returns(book);

            // Act
            var result = _controller.GetBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(book.Id, returnBook.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetBook_ShouldReturnNotFound_WhenBookDoesNotExist(int bookId)
        {
            // Arrange
            _mockBookService.Setup(service => service.GetBookById(bookId)).Returns((Book)null);

            // Act
            var result = _controller.GetBook(bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = notFoundResult.Value;
            Assert.Equal($"Book with id {bookId} not found", errorResponse.GetType().GetProperty("Message").GetValue(errorResponse, null));
        }


        [Fact]
        public void AddBook_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book", Author = "Test Author", Year = 2021 };

            // Act
            var result = _controller.AddBook(book);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = createdAtActionResult.Value;
            Assert.Equal("Book added successfully", response.GetType().GetProperty("Message").GetValue(response, null));
            Assert.Equal(book.Title, response.GetType().GetProperty("book").GetValue(response, null).GetType().GetProperty("Title").GetValue(response.GetType().GetProperty("book").GetValue(response, null), null));
        }

        [Fact]
        public void AddBook_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            var book = new Book { Author = "Test Author", Year = 2021 };

            // Act
            var result = _controller.AddBook(book);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void UpdateBook_ShouldReturnOk_WhenBookIsUpdated()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Updated Test Book", Author = "Updated Test Author", Year = 2021 };
            _mockBookService.Setup(service => service.GetBookById(1)).Returns(book);

            // Act
            var result = _controller.UpdateBook(1, book);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Assert.Equal("Book updated successfully", response.GetType().GetProperty("Message").GetValue(response, null));
            Assert.Equal(book.Title, response.GetType().GetProperty("book").GetValue(response, null).GetType().GetProperty("Title").GetValue(response.GetType().GetProperty("book").GetValue(response, null), null));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        public void UpdateBook_ShouldReturnBadRequest_WhenIdMismatch(int bookId, int updateId)
        {
            // Arrange
            var book = new Book { Id = bookId, Title = "Updated Test Book", Author = "Updated Test Author", Year = 2021 };

            // Act
            var result = _controller.UpdateBook(updateId, book);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = badRequestResult.Value;
            Assert.Equal("Book id mismatch", errorResponse.GetType().GetProperty("Message").GetValue(errorResponse, null));
        }

        [Fact]
        public void UpdateBook_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            var book = new Book { Id = 1, Author = "Updated Test Author", Year = 2021 };

            // Act
            var result = _controller.UpdateBook(1, book);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteBook_ShouldReturnOk_WhenBookIsDeleted(int bookId)
        {
            // Arrange
            var book = new Book { Id = bookId, Title = "Test Book", Author = "Test Author", Year = 2021 };
            _mockBookService.Setup(service => service.GetBookById(bookId)).Returns(book);

            // Act
            var result = _controller.DeleteBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            Assert.Equal("Book deleted successfully", response.GetType().GetProperty("Message").GetValue(response, null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteBook_ShouldReturnNotFound_WhenBookDoesNotExist(int bookId)
        {
            // Arrange
            _mockBookService.Setup(service => service.GetBookById(bookId)).Returns((Book)null);

            // Act
            var result = _controller.DeleteBook(bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = notFoundResult.Value;
            Assert.Equal($"Book with id {bookId} not found", errorResponse.GetType().GetProperty("Message").GetValue(errorResponse, null));
        }
    }
}
