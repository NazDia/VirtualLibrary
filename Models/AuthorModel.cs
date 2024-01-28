using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Models;


[PrimaryKey(nameof(Id))]
public class AuthorModel {
    [Required]
    public long Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string Nationality { get; set; } = "";
    [Required]
    public DateTime BirthDate { get; set; }
    public ICollection<BookModel> Books { get; set; }
    public ICollection<SubsriptionModel> Subsriptions { get; set; }
}

public class CreateAuthorModel {
    public string Name { get; set; } = "";
    public string Nationality { get; set; } = "";
    public DateTime BirthDate { get; set; }
}

public class ShowAuthorModel {
    internal long Id { get; set; }
    public string Name { get; set; } = "";
    public string Nationality { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public int SubsriptionCount { get; set; }
    public IEnumerable<ShowBookAuthoredModel> Books { get; set; }
}