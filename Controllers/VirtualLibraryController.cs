using Microsoft.AspNetCore.Mvc;
using VirtualLibrary.Models;

namespace VirtualLibrary.Controllers;

[ApiController]
[Route("api/v1.0/library")]
public class VirtualLibraryController : ControllerBase {
    private readonly IConfiguration _config;
    public VirtualLibraryController(IConfiguration configuration)
    {
        _config = configuration;
    }

    // POST /api/v1.0/library/users
    [HttpPost("users")]
    public async Task<ActionResult<LibraryUserModel>> CreateUser([FromBody] LibraryUserModel user) {
        throw new NotImplementedException();
    }

    // PUT /api/v1.0/library/users
    [HttpPut("users/{userId}")]
    public async Task<ActionResult<LibraryUserModel>> UpdatePfp(long userId, [FromBody] string pfp_url) {
        throw new NotImplementedException();
    }

    // DELETE /api/v1.0/library/users
    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(long userId) {
        throw new NotImplementedException();
    }

    // GET /api/v1.0/library/users?offset=int&limit=int
    [HttpGet("users")]
    public async Task<ActionResult<ListModels<LibraryUserModel>>> ListUsers(int offset, int limit) {
        throw new NotImplementedException();
    }

    // POST /api/v1.0/library/users/{userId}/subscribe-to-author/{authorId}
    [HttpPost("users/{userId}/subscribe-to-author/{authorId}")]
    public async Task<IActionResult> SubscribeToAuthor(long userId, long authorId) {
        throw new NotImplementedException();
    }

    // DELETE /api/v1.0/library/users/{userId}/subscribe-to-author/{authorId}
    [HttpDelete("users/{userId}/subscribe-to-author/{authorId}")]
    public async Task<IActionResult> Unsubscribe(long userId, long authorId) {
        throw new NotImplementedException();
    }

    // POST /api/v1.0/library/authors
    [HttpPost("authors")]
    public async Task<ActionResult<AuthorModel>> CreateAuthor([FromBody] AuthorModel authorModel) {
        throw new NotImplementedException();
    }

    // GET /api/v1.0/library/authors/{authorId}
    [HttpGet("authors/{authorId}")]
    public async Task<ActionResult<AuthorModel>> GetAuthor(long authorId) {
        throw new NotImplementedException();
    }

    // GET /api/v1.0/library/books?authorId=long?&editorialName=string?&before=DateTime?&after=DateTime?&offset=int&limit=int&sort=bool?
    [HttpGet("books")]
    public async Task<ActionResult<ListModels<BookModel>>> ListBooks(
        long? authorId,
        string? editorialName,
        DateTime? before,
        DateTime? after,
        int offset,
        int limit,
        bool? sort
    ) {
        throw new NotImplementedException();
    }

    // POST /api/v1.0/library/authors/{authorId}/books
    [HttpPost("authors/{authorId}/books")]
    public async Task<ActionResult<BookModel>> CreateBook(long authorId, [FromBody] BookModel bookModel) {
        throw new NotImplementedException();
    }

    // GET /api/v1.0/library/books/reviews?reviewType=int?&sort=bool?&offset=int&limit=int
    [HttpGet("books/{bookId}/reviews")]
    public async Task<ActionResult<ListModels<ReviewModel>>> ListReviews(long bookId, int? reviewType, bool? sort, int offset, int limit) {
        throw new NotImplementedException();
    }

    // POST /api/v1.0/library/books/{bookId}/reviews/from/user/{userId}
    [HttpPost("books/{bookId}/reviews/from/user/{userId}")]
    public async Task<IActionResult> SetReview(long bookId, long userId) {
        throw new NotImplementedException();
    }
}