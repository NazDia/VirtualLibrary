namespace VirtualLibrary.Models;

public class ListModels<T> {
    public int Offset { get; set; }
    public int Limit { get; set; }
    public ICollection<T> Elements { get; set; }
}