using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunWithin.Models
{
    public class EFReviewRepository : IReviewRepository
    {
        private ApplicationDbContextPostgres context;
        public EFReviewRepository (ApplicationDbContextPostgres ctx)
        {
            context = ctx;
        }
        public IQueryable<Review> Reviews => context.Reviews;
        public void SaveReview(Review review)
        {
            if(review.ID == 0)
            {
                context.Reviews.Add(review);
            }
            context.SaveChanges();
        }
        
    }
}
