namespace VirtualLibrary.Models;

public class AuthorModel {
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Nationality { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public ICollection<BookModel> Books { get; set; } = new List<BookModel>();
    public ICollection<SubsriptionModel> Subsriptions { get; set; } = new List<SubsriptionModel>();
}

public class CreateAuthorModel {
    public string Name { get; set; } = "";
    public string Nationality { get; set; } = "";
    public DateTime BirthDate { get; set; }
}