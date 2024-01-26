using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Models;

public class LibraryUserModel {
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Pfp_url { get; set; }
    public ICollection<ReviewModel> Reviews { get; set; }
    public ICollection<SubsriptionModel> Subsriptions { get; set; }
}

[PrimaryKey(nameof(UserId), nameof(AuthorId))]
public class SubsriptionModel {
    public long UserId { get; set; }
    public LibraryUserModel User { get; set; }
    public long AuthorId { get; set; }
    public AuthorModel Author { get; set; }
}

public class CreateUserModel {
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Pfp_url { get; set; }
}