namespace VirtualLibrary.Models;


public class AuthorModel {
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Nationality { get; set; } = "";
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
    public string Name { get; set; } = "";
    public string Nationality { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public int SubsriptionCount { get; set; }
    public ICollection<ShowBookAuthoredModel> Books { get; set; }
}