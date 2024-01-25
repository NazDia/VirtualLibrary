namespace VirtualLibrary.Interfaces;

public interface IRepository<T> {
    public async Task<T> GetInstance() {
        throw new NotImplementedException();
    }
}