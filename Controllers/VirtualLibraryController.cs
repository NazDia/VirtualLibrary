using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;
using VirtualLibrary.Utils;

namespace VirtualLibrary.Controllers;

[ApiController]
[Route("api/v1.0/library")]
public class VirtualLibraryController : ControllerBase {
    private readonly IConfiguration _config;
    private readonly IVirtualLibraryInterface _repository;
    private readonly IEmailSender _email;
    public VirtualLibraryController(IConfiguration configuration, IVirtualLibraryInterface repository, IEmailSender emailSender)
    {
        _config = configuration;
        _repository = repository;
        _email = emailSender;
    }

    // POST /api/v1.0/library/users
    [HttpPost("users")]
    public async Task<ActionResult<ShowUserModel>> CreateUser([FromBody] CreateUserModel user) {
        if (!MyValidators.IsNotEmpty(user.Name)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsValidEmail(user.Email)) return BadRequest(MyValidators.InvalidEmail);
        if (user.Pfp_url == null) user.Pfp_url = "";
        if (user.Pfp_url != "" && !MyValidators.IsValidUri(user.Pfp_url)) return BadRequest(MyValidators.InvalidUrl);
        var wuser = await _repository.CreateUser(user);
        return Ok(wuser);
    }

    // PUT /api/v1.0/library/users/{userId}
    [HttpPut("users/{userId}")]
    public async Task<ActionResult<ShowUserModel>> UpdatePfp(long userId, [FromBody] string? pfp_url) {
        if (pfp_url == null) pfp_url = "";
        if (pfp_url != "" && !MyValidators.IsValidUri(pfp_url)) return BadRequest(MyValidators.InvalidUrl);
        var found = await _repository.SetPfp(userId, pfp_url);
        if (found == null) return NotFound();
        return Ok(found);
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
    public async Task<ActionResult<ListModels<ShowUserModel>>> ListUsers(int offset, int limit) {
        if (!MyValidators.IsValidInterval(offset, limit)) return BadRequest(MyValidators.InvalidInterval);
        var listModels = await _repository.ListUsers(offset, limit);
        return Ok(listModels);
    }

    // POST /api/v1.0/library/users/{userId}/subscribe-to-author/{authorId}
    [HttpPost("users/{userId}/subscribe-to-author/{authorId}")]
    public async Task<IActionResult> SubscribeToAuthor(long userId, long authorId) {
        var found = await _repository.CreateSubscription(userId, authorId);
        if (found == null) return BadRequest();
        if (!(bool)found) return NotFound();
        return NoContent();
    }

    // DELETE /api/v1.0/library/users/{userId}/subscribe-to-author/{authorId}
    [HttpDelete("users/{userId}/subscribe-to-author/{authorId}")]
    public async Task<IActionResult> Unsubscribe(long userId, long authorId) {
        var found = await _repository.DeleteSubscription(userId, authorId);
        if (found == null) return BadRequest();
        if (!(bool)found) return NotFound();
        return NoContent();
    }

    // POST /api/v1.0/library/authors
    [HttpPost("authors")]
    public async Task<ActionResult<ShowAuthorModel>> CreateAuthor([FromBody] CreateAuthorModel authorModel) {
        if (!MyValidators.IsNotEmpty(authorModel.Name)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsNotEmpty(authorModel.Nationality)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsValidAge(authorModel.BirthDate)) return BadRequest(MyValidators.InvalidAge);
        var author = await _repository.CreateAuthor(authorModel);
        return Ok(author);
    }

    // GET /api/v1.0/library/authors/{authorId}
    [HttpGet("authors/{authorId}")]
    public async Task<ActionResult<ShowAuthorModel>> GetAuthor(long authorId) {
        var ret = await _repository.DetailsAuthor(authorId);
        if (ret == null) return NotFound();
        return Ok(ret);
    }

    // GET /api/v1.0/library/books?authorId=long?&editorialName=string?&before=DateTime?&after=DateTime?&offset=int&limit=int&sort=bool?
    [HttpGet("books")]
    public async Task<ActionResult<ListModels<ShowBookListedModel>>> ListBooks(
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
            limit,
            sort
        );
        return Ok(ret);
    }

    // POST /api/v1.0/library/authors/{authorId}/books
    [HttpPost("authors/{authorId}/books")]
    public async Task<ActionResult<ShowBookListedModel>> CreateBook(long authorId, [FromBody] CreateBookModel bookModel) {
        if (!MyValidators.IsNotEmpty(bookModel.Editorial)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsNotEmpty(bookModel.Name)) return BadRequest(MyValidators.InvalidEmpty);
        if (!MyValidators.IsValidPageCount(bookModel.Pages)) return BadRequest(MyValidators.InvalidPagesCount);
        if (!MyValidators.IsValidIsbn(bookModel.Isbn)) return BadRequest(MyValidators.InvalidIsbn);
        if (!MyValidators.IsValidUri(bookModel.Url)) return BadRequest(MyValidators.InvalidUrl);
        var ret = await _repository.CreateBook(authorId, bookModel);
        if (ret == null) return NotFound();
        var emails = await _repository.GetSubscriptorsEmails(authorId);
        foreach (var email in emails) {
            _ = _email.SendEmailAsync(
                _config["EmailSender:Source"],
                email,
                _config["EmailSender:Topic"],
                _config["EmailSender:Text"]
            );
        }
        return Ok(ret);
    }

    // GET /api/v1.0/library/books/reviews?reviewType=int?&sort=bool?&offset=int&limit=int
    [HttpGet("books/{bookId}/reviews")]
    public async Task<ActionResult<ListModels<ShowReviewModel>>> ListReviews(
        long bookId,
        int? reviewType,
        bool? sort,
        int offset,
        int limit
    ) {
        if (reviewType != null && !MyValidators.IsValidQualification((int)reviewType)) return BadRequest(MyValidators.InvalidQualification);
        if (!MyValidators.IsValidInterval(offset, limit)) return BadRequest(MyValidators.InvalidInterval);
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