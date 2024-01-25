namespace VirtualLibrary.Models;

public class AuthorModel {
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Nationality { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public ICollection<BookModel> Books { get; set; }
}