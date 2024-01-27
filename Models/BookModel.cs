using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Models;

[PrimaryKey(nameof(Id))]
public class BookModel {
    internal long Id { get; set; }
    public string Name { get; set; } = "";
    // public string Author { get; set; } = "";
    public string Editorial { get; set; } = "";
    public int Pages { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Url { get; set; } = "";
    public string Isbn { get; set; } = "";
    public int Qualification { get; set; }
    public long AuthorModelId { get; set; }
    public AuthorModel AuthorModel { get; set; }
    public ICollection<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();
}

public class CreateBookModel {
    public string Name { get; set; } = "";
    public string Editorial { get; set; } = "";
    public int Pages { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Isbn { get; set; } = "";
    public string Url { get; set; } = "";
}