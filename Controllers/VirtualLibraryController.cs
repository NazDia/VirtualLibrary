using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;

namespace VirtualLibrary.Controllers;

[ApiController]
[Route("api/v1.0/library")]
public class VirtualLibraryController : ControllerBase {
    private readonly IConfiguration _config;
    private readonly IVirtualLibraryInterface _repository;
    public VirtualLibraryController(IConfiguration configuration, IVirtualLibraryInterface repository)
    {
        _config = configuration;
        _repository = repository;
    }

    // POST /api/v1.0/library/users
    [HttpPost("users")]
    public async Task<ActionResult<LibraryUserModel>> CreateUser([FromBody] CreateUserModel user) {
        _repository.CreateUser(user);
        return NoContent();
    }

    // PUT /api/v1.0/library/users/{userId}
    [HttpPut("users/{userId}")]
    public async Task<ActionResult<LibraryUserModel>> UpdatePfp(long userId, [FromBody] string pfp_url) {
        var found = await _repository.SetPfp(userId, pfp_url);
        if (!found) return NotFound();
        return NoContent();
    }

    // DELETE /api/v1.0/library/users/{userId}
    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(long userId) {
        var found = await _repository.DeleteUser(userId);
        if (!found) return NotFound();
        return NoContent();
    }

    // GET /api/v1.0/library/users?offset=int&limit=int
    [HttpGet("users")]
    public async Task<ActionResult<ListModels<LibraryUserModel>>> ListUsers(int offset, int limit) {
        var listModels = await _repository.ListUsers(offset, limit);
        return Ok(listModels);
    }

    // POST /api/v1.0/library/users/{userId}/subscribe-to-author/{authorId}
    [HttpPost("users/{userId}/subscribe-to-author/{authorId}")]
    public async Task<IActionResult> SubscribeToAuthor(long userId, long authorId) {
        _repository.CreateSubscription(userId, authorId);
        return NoContent();
    }

    // DELETE /api/v1.0/library/users/{userId}/subscribe-to-author/{authorId}
    [HttpDelete("users/{userId}/subscribe-to-author/{authorId}")]
    public async Task<IActionResult> Unsubscribe(long userId, long authorId) {
        var found = await _repository.DeleteSubscription(userId, authorId);
        if (!found) return NotFound();
        return NoContent();
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