namespace VirtualLibrary.Models;

public class LibraryUserModel {
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Pfp_url { get; set; }
    public ICollection<ReviewModel> Reviews { get; set; }
}
