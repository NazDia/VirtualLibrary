using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;

namespace VirtualLibrary.Mappers;

public class CreateReviewMapper : IMapper<CreateReviewModel, ReviewModel>
{
    public ReviewModel map(CreateReviewModel obj)
    {
        return new ReviewModel {
            Qualification = obj.Qualification,
            Review = obj.Description,
            CreationTime = DateTime.Now
        };
    }

    public CreateReviewModel rmap(ReviewModel robj)
    {
        throw new NotImplementedException();
    }
}

public class ShowReviewMapper : IMapper<ReviewModel, ShowReviewModel>
{
    public ShowReviewModel map(ReviewModel obj)
    {
        return new ShowReviewModel {
            Review = obj.Review,
            Qualification = obj.Qualification,
            CreationTime = obj.CreationTime,
            User = (new ShowUserNaiveMapper()).map(obj.LibraryUserModel)
        };
    }

    public ReviewModel rmap(ShowReviewModel robj)
    {
        throw new NotImplementedException();
    }
}