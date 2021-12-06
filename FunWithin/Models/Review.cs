using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace FunWithin.Models
{
    public class Review
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please choose the type")]
        public string Type { get; set; }
        public string ReviewAuthor { get; set; }
        public string ReviewAuthorID { get; set; }
        [Required(ErrorMessage = "Please enter the name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the author")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Please choose the genre")]
        public string Genre { get; set; }
        [Required(ErrorMessage = "Please enter the review text")]
        public string ReviewText { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Likes { get; set; }
        public List<string> ReviewLikedUsers { get; set; }
        public decimal AverageItemGrade { get; set; }
        [Required(ErrorMessage = "Please rate it!")]
        public List<int> ItemGrade { get; set; }
        public List<string> GradedUser { get; set; }
        public string[] Cover { get; set; }
        
        public Review()
        {
            PublicationDate = DateTime.Now;
            Likes = 0;
        }
    }
}
