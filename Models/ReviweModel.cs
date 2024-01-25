using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Models;

[PrimaryKey(nameof(LibraryUserModelId), nameof(BookModelId))]
public class ReviewModel {
    public long LibraryUserModelId { get; set; }
    public LibraryUserModel LibraryUserModel { get; set; }
    public long BookModelId { get; set; }
    public BookModel BookModel { get; set; }
    public DateTime CreationTime { get; set; }
    public string Review { get; set; } = "";
    public int Qualification { get; set; }
}