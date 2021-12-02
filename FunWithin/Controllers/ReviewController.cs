using Microsoft.AspNetCore.Mvc;
using FunWithin.Models;
using System.Linq;
using System.IO;
using System.Web;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using FunWithin.Models.ViewModels;

namespace FunWithin.Controllers
{
    public class ReviewController : Controller
    {
        private IReviewRepository repository;
        IWebHostEnvironment environment;
        //private Review review;
        public int PageSize = 4;

        public ReviewController(IReviewRepository repo, IWebHostEnvironment env)
        {
            repository = repo;
            environment = env;
        }
        public ViewResult List(string type, string sortBy, int reviewPage = 1)
        {
            ViewBag.DateSort = (String.IsNullOrEmpty(sortBy) | sortBy == "PublicationDate") ?
                "" : "Date";
            ViewBag.LikeSort = "Like";
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
        private static IQueryable<Review> SwitchSort(string sortBy, IQueryable<Review> Reviews)
        {
            switch (sortBy)
            {
                case "Like":
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
        public IActionResult NewReview(Review review)
        {
            if (ModelState.IsValid)
            {
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
        [HttpPost]
        public ActionResult BatchUpload()
        {
            bool uploadSuccessfully = true;
            int count = 0;
            string  msg = "", 
                    fileName = "", 
                    fileExtension = "", 
                    filePath = "", 
                    fileNewName = "";

            try
            {
                string directoryPath = environment.WebRootPath + "/Content/cover/ ";
                ViewBag.DirPath = directoryPath;
                if(!Directory.Exists(directoryPath))
                { Directory.CreateDirectory(directoryPath); }
                foreach(var f in Request.Form.Files)
                {
                    //IFormFile file = Request.Form.Files[f];
                    if (f.Length > 0)
                    {
                        fileName = f.FileName;
                        fileExtension = Path.GetExtension(fileName);
                        fileNewName = Guid.NewGuid().ToString() + fileExtension;
                        filePath = Path.Combine(directoryPath, fileNewName);
                        using(Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            f.CopyToAsync(fileStream);
                        }
                        count++;
                        
                    }
                }
                    
            }
            catch(Exception e)
            {
                msg = e.Message;
                uploadSuccessfully = false;
            }
            return Json(new
            {
                Result = uploadSuccessfully,
                Count = count,
                Message = msg
            });
        }
    }

    }

