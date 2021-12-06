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
            if (review.ID == 0)
            {
                AverageGrade(review);
                context.Reviews.Add(review);
            }
            else
            {
                Review dbEntry = context.Reviews
                    .FirstOrDefault(r => r.ID == review.ID);
                    if(dbEntry != null)
                    {
                        dbEntry.Name = review.Name;
                        dbEntry.Type = review.Type;
                        dbEntry.Genre = review.Genre;
                        dbEntry.Author = review.Author;
                        dbEntry.ReviewText = review.ReviewText;
                        dbEntry.ItemGrade = review.ItemGrade;
                    }
             
            }

            context.SaveChanges();
        }
        public Review DeleteReview(int ID)
        {
            Review dbEntry = context.Reviews
                .FirstOrDefault(r => r.ID == ID);
            if(dbEntry != null)
            {
                context.Reviews.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        private void AverageGrade(Review review)
        {
            review.AverageItemGrade = (review.ItemGrade.Sum(g => g)) / review.ItemGrade.Count;
            
        }
    }
}
