using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Models;

[PrimaryKey(nameof(Id))]
public class LibraryUserModel {
    [Required]
    public long Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = "";
    [StringLength(200)]
    public string? Pfp_url { get; set; }
    [Required]
    public DateTime CreationTime { get; set; }
    public ICollection<ReviewModel> Reviews { get; set; }
    public ICollection<SubsriptionModel> Subsriptions { get; set; }
}

[PrimaryKey(nameof(UserId), nameof(AuthorId))]
public class SubsriptionModel {
    [Required]
    public long UserId { get; set; }
    public LibraryUserModel User { get; set; }
    [Required]
    public long AuthorId { get; set; }
    public AuthorModel Author { get; set; }
}

public class CreateUserModel {
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Pfp_url { get; set; }
}

public class ShowUserModel {
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Pfp_url { get; set; }
    public DateTime CreationTime { get; set; }
    public int SubscriptionCount { get; set; }
}

public class ShowUserNaive {
    public long Id { get; set; }
    public string Name { get; set; } = "";
}