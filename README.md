# BookCatalogRestAPI

A RESTful API for managing a catalog of books. Built with .NET 8, C#, and Serilog for logging. This project includes endpoints for creating, reading, updating, and deleting books, as well as comprehensive unit tests using xUnit and Moq.

## Features

- Create, read, update, and delete books
- Logging with Serilog
- Unit tests with xUnit and Moq

### API Endpoints

The API will be available at `https://localhost:7298` or `http://localhost:5215`.

- `GET /api/books/get-all-books` - Get details of all books
- `GET /api/books/book/{id}` - Get details of a specific book by ID
- `POST /api/books/add` - Add a new book
- `PUT /api/books/update/{id}` - Update a book by ID
- `DELETE /api/books/delete/{id}` - Delete a book by ID
