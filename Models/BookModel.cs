using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Models;


[PrimaryKey(nameof(Id))]
public class BookModel {
    [Required]
    internal long Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "";
    [Required]
    [StringLength(20)]
    public string Isbn { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string Editorial { get; set; } = "";
    [Required]
    [Range(10, int.MaxValue)]
    public int Pages { get; set; }
    [Required]
    [StringLength(200)]
    public string Url { get; set; } = "";
    [Required]
    public DateTime PublicationDate { get; set; }
    public int Qualification { get; set; }
    [Required]
    public long AuthorModelId { get; set; }
    public AuthorModel AuthorModel { get; set; }
    public ICollection<ReviewModel> Reviews { get; set; }
}

public class CreateBookModel {
    public string Name { get; set; } = "";
    public string Isbn { get; set; } = "";
    public string Editorial { get; set; } = "";
    public int Pages { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Url { get; set; } = "";
}


public class ShowBookListedModel {
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Isbn { get; set; } = "";
    public string Editorial { get; set; } = "";
    public string AuthorName { get; set; } = "";
    public int Qualification { get; set; }
}

public class ShowBookAuthoredModel {
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Isbn { get; set; } = "";
    public DateTime PublicationDate { get; set; }
}