using Microsoft.AspNetCore.Mvc;
using FunWithin.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using FunWithin.Models.ViewModels;

namespace FunWithin.Controllers
{
    public class ReviewController : Controller
    {
        private IReviewRepository repository;
        private UserManager<User> userManager;
        public int PageSize = 4;

        public ReviewController(IReviewRepository repo, UserManager<User> usrManager)
        {
            repository = repo;
            userManager = usrManager;
        }
        public ViewResult List(string type, string sortBy, int reviewPage = 1)
        {
            ViewBag.DateSort = (String.IsNullOrEmpty(sortBy) | sortBy == "PublicationDate") ?
                "" : "Date";
            ViewBag.LikeSort = "Likes";
            var reviews = repository.Reviews.Where(r => type == null || r.Type == type);

            reviews = SwitchSort(sortBy, reviews);
            return View(new ReviewListViewModel
            {
                Reviews = reviews
                .Skip((reviewPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = reviewPage,
                    ItemsPerPage = PageSize,
                    TotalItems = type == null ?
                    repository.Reviews.Count() :
                    repository.Reviews.Where(e =>
                    e.Type == type).Count()
                },
                CurrentType = type,
            });
        }
        [Authorize]
        [HttpPost]
        public IActionResult Like(int ID)
        {
            
            Review review = GetReviewByID(ID);
            if (review != null)
            {
                review.Likes++;
                repository.SaveReview(review);
            }
            return RedirectToAction(nameof(List));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Rate(int ID, int grade)
        {
            Review review = GetReviewByID(ID);
            if (review != null)
            {
                review.ItemGrade.Add(grade);
                review.AverageItemGrade = Convert.ToDecimal(review.ItemGrade.Sum(g => g)) / review.ItemGrade.Count;
                review.AverageItemGrade = Decimal.Round(review.AverageItemGrade, 1);
                repository.SaveReview(review);
            }
            return RedirectToAction(nameof(List));
        }
        public ViewResult ShowReview(int ID) =>
            View(GetReviewByID(ID));
        [HttpPost]
        public IActionResult ShowReview(Review review)
        {

            return View(review);
        }
        private Review GetReviewByID(int ID)
        {
            return repository.Reviews
                .FirstOrDefault(r => r.ID == ID);
        }
        private static IQueryable<Review> SwitchSort(string sortBy, IQueryable<Review> Reviews)
        {
            switch (sortBy)
            {
                case "Likes":
                    Reviews = Reviews.OrderByDescending(r => r.Likes);
                    break;
                default:
                    Reviews = Reviews.OrderByDescending(r => r.PublicationDate);
                    break;
            }
            return Reviews;
        }
        public ViewResult NewReview() => View(new Review());
        [HttpPost]
        public IActionResult NewReview(Review review, User user)
        {
            if (ModelState.IsValid)
            {
                review.ReviewAuthor = user.UserName;
                review.ReviewAuthorID = user.Id;
                repository.SaveReview(review);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(review);
            }
        }
            [HttpGet]
        public IActionResult Autocomlplete()
        {
            var inputGenre = HttpContext.Request.Query["term"].ToString();
            var genre = repository.Reviews.Where(r => r.Genre.Contains(inputGenre))
                .Select(j => j.Genre).Distinct().ToList();
            return Ok(genre);
        }
        [HttpPost]
        public ViewResult GenreTag()
        {
            return View(repository.Reviews
                .Select(r => r.Genre)
                .Distinct()
                .OrderBy(r => r));
        }
        public ViewResult Completed()
        {
            return View();
        }
    }
}

