using System.Linq;

namespace FunWithin.Models
{
    public interface IReviewRepository
    {
        IQueryable<Review> Reviews { get; }
        void SaveReview(Review review);
        Review DeleteReview(int ID);
    }
}
