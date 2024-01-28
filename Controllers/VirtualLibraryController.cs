using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;
using VirtualLibrary.Validators;

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
        if (!MyValidators.IsNotEmpty(user.Name)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsValidEmail(user.Email)) return BadRequest(MyValidators.InvalidEmail);
        if (user.Pfp_url != null && !MyValidators.IsValidUri(user.Pfp_url)) return BadRequest(MyValidators.InvalidUrl);
        _repository.CreateUser(user);
        return NoContent();
    }

    // PUT /api/v1.0/library/users/{userId}
    [HttpPut("users/{userId}")]
    public async Task<ActionResult<LibraryUserModel>> UpdatePfp(long userId, [FromBody] string pfp_url) {
        if (!MyValidators.IsValidUri(pfp_url)) return BadRequest(MyValidators.InvalidUrl);
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
        if (!MyValidators.IsValidInterval(offset, limit)) return BadRequest(MyValidators.InvalidInterval);
        var listModels = await _repository.ListUsers(offset, limit);
        return Ok(listModels);
    }

    // POST /api/v1.0/library/users/{userId}/subscribe-to-author/{authorId}
    [HttpPost("users/{userId}/subscribe-to-author/{authorId}")]
    public async Task<IActionResult> SubscribeToAuthor(long userId, long authorId) {
        var found = await _repository.CreateSubscription(userId, authorId);
        if (!found) return NotFound();
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
    public async Task<ActionResult<AuthorModel>> CreateAuthor([FromBody] CreateAuthorModel authorModel) {
        if (!MyValidators.IsNotEmpty(authorModel.Name)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsNotEmpty(authorModel.Nationality)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsValidAge(authorModel.BirthDate)) return BadRequest(MyValidators.InvalidAge);
        _repository.CreateAuthor(authorModel);
        return NoContent();
    }

    // GET /api/v1.0/library/authors/{authorId}
    [HttpGet("authors/{authorId}")]
    public async Task<ActionResult<AuthorModel>> GetAuthor(long authorId) {
        var ret = await _repository.DetailsAuthor(authorId);
        if (ret == null) return NotFound();
        return Ok(ret);
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
        if (!MyValidators.IsValidInterval(offset, limit)) return BadRequest(MyValidators.InvalidInterval);
        var ret = await _repository.ListBooks(
            authorId,
            editorialName,
            before,
            after,
            offset,
            limit
        );
        return Ok(ret);
    }

    // POST /api/v1.0/library/authors/{authorId}/books
    [HttpPost("authors/{authorId}/books")]
    public async Task<ActionResult<BookModel>> CreateBook(long authorId, [FromBody] CreateBookModel bookModel) {
        if (!MyValidators.IsNotEmpty(bookModel.Editorial)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsNotEmpty(bookModel.Name)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsValidPageCount(bookModel.Pages)) return BadRequest(MyValidators.InvalidPagesCount);
        if (!MyValidators.IsValidIsbn(bookModel.Isbn)) return BadRequest(MyValidators.InvalidIsbn);
        var ret = await _repository.CreateBook(authorId, bookModel);
        if (ret == null) return NotFound();
        return Ok(ret);
    }

    // GET /api/v1.0/library/books/reviews?reviewType=int?&sort=bool?&offset=int&limit=int
    [HttpGet("books/{bookId}/reviews")]
    public async Task<ActionResult<ListModels<ShowReviewModel>>> ListReviews(
        long bookId,
        int? reviewType,
        bool? sort,
        int offset,
        int limit) {
        var ret = await _repository.ListReviews(bookId, reviewType, sort, offset, limit);
        if (ret == null) return NotFound();
        return Ok(ret);
    }

    // POST /api/v1.0/library/books/{bookId}/reviews/from/user/{userId}
    [HttpPost("books/{bookId}/reviews/from/user/{userId}")]
    public async Task<ActionResult<ShowReviewModel>> SetReview(long bookId, long userId, [FromBody] CreateReviewModel createReviewModel) {
        if (!MyValidators.IsValidQualification(createReviewModel.Qualification)) return BadRequest(MyValidators.InvalidQualification);
        var ret = await _repository.CreateReview(bookId, userId, createReviewModel);
        if (ret == null) return NotFound();
        return Ok(ret);
    }
}