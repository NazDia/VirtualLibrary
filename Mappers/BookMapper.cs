using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;

namespace VirtualLibrary.Mappers;

public class CreateBookMapper : IMapper<CreateBookModel, BookModel>
{
    public BookModel map(CreateBookModel obj)
    {
        return new BookModel {
            Id = 0,
            PublicationDate = obj.PublicationDate,
            Name = obj.Name,
            Pages = obj.Pages,
            Url = obj.Url,
            Isbn = obj.Isbn,
            Editorial = obj.Editorial
        };
    }

    public CreateBookModel rmap(BookModel robj)
    {
        throw new NotImplementedException();
    }
}

public class ShowBookListedMapper : IMapper<BookModel, ShowBookListedModel>
{
    public ShowBookListedModel map(BookModel obj)
    {
        return new ShowBookListedModel {
            Name = obj.Name,
            AuthorName = obj.AuthorModel.Name,
            Editorial = obj.Editorial,
            Isbn = obj.Isbn,
            Id = obj.Id
        };
    }

    public BookModel rmap(ShowBookListedModel robj)
    {
        throw new NotImplementedException();
    }
}

public class ShowBookAuthoredMapper : IMapper<BookModel, ShowBookAuthoredModel>
{
    public ShowBookAuthoredModel map(BookModel obj)
    {
        return new ShowBookAuthoredModel {
            Name = obj.Name,
            PublicationDate = obj.PublicationDate,
            Isbn = obj.Isbn,
            Id = obj.Id
        };
    }

    public BookModel rmap(ShowBookAuthoredModel robj)
    {
        throw new NotImplementedException();
    }
}