using System.Collections.Generic;
using System;

namespace FunWithin.Models
{
    public class Review
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string ReviewAuthor { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string[] ReviewText { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Likes { get; set; }
        public int[] ReviewLikesUsersId { get; set; }
        public decimal AverageItemRating { get; set; }
        public int[] ItemGrade { get; set; }
        public int[] GradeUserId { get; set; }
        public string[] Cover { get; set; }
        public Review()
        {
            PublicationDate = DateTime.Now;
        }

    }
}
