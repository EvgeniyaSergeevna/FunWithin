using Microsoft.AspNetCore.Mvc;
using FunWithin.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunWithin.Controllers
{
    public class DataController : Controller
    {
        private IReviewRepository repository;
        private Review review;

        public DataController(IReviewRepository repoServices, Review reviewServices)
        {
            repository = repoServices;
            review = reviewServices;
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
        public ViewResult Completed()
        {
            return View();
        }
    }
}
