using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;

namespace VirtualLibrary.Mappers;

public class CreateAuthorMapper : IMapper<CreateAuthorModel, AuthorModel>
{
    public AuthorModel map(CreateAuthorModel obj)
    {
        return new AuthorModel {
            Name = obj.Name,
            Id = 0,
            BirthDate = obj.BirthDate,
            Nationality = obj.Nationality
        };
    }

    public CreateAuthorModel rmap(AuthorModel robj)
    {
        throw new NotImplementedException();
    }
}

public class ShowAuthorMapper : IMapper<AuthorModel, ShowAuthorModel>
{
    private readonly IMapper<BookModel, ShowBookAuthoredModel> _mapper;
    public ShowAuthorMapper()
    {
        _mapper = new ShowBookAuthoredMapper();
    }
    public ShowAuthorModel map(AuthorModel obj)
    {
        return new ShowAuthorModel {
            Name = obj.Name,
            BirthDate = obj.BirthDate,
            Nationality = obj.Nationality,
            SubsriptionCount = obj.Subsriptions.Count,
            Books = _mapper.lMap(obj.Books)
        };
    }

    public AuthorModel rmap(ShowAuthorModel robj)
    {
        throw new NotImplementedException();
    }
}