using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;

namespace VirtualLibrary.Mappers;

public class UserCreationMapper : IMapper<CreateUserModel, LibraryUserModel> {
    public LibraryUserModel map(CreateUserModel obj) {
        return new LibraryUserModel {
            Id = 0,
            Name = obj.Name,
            CreationTime = DateTime.Now,
            Email = obj.Email,
            Pfp_url = obj.Pfp_url
        };
    }

    public CreateUserModel rmap(LibraryUserModel robj) {
        throw new NotImplementedException();
    }
}

public class ShowUserMapper : IMapper<LibraryUserModel, ShowUserModel> {
    public ShowUserModel map(LibraryUserModel obj) {
        return new ShowUserModel {
            Id = obj.Id,
            Name = obj.Name,
            Email = obj.Email,
            Pfp_url = obj.Pfp_url,
            CreationTime = obj.CreationTime,
            SubscriptionCount = obj.Subsriptions.Count
        };
    }

    public LibraryUserModel rmap(ShowUserModel robj) {
        throw new NotImplementedException();
    }
}
public class ShowUserNaiveMapper : IMapper<LibraryUserModel, ShowUserNaive> {
    public ShowUserNaive map(LibraryUserModel obj) {
        return new ShowUserNaive {
            Id = obj.Id,
            Name = obj.Name
        };
    }

    public LibraryUserModel rmap(ShowUserNaive robj) {
        throw new NotImplementedException();
    }
}