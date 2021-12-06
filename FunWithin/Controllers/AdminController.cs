using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using FunWithin.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using FunWithin.Models.ViewModels;

namespace FunWithin.Controllers
{
    public class AdminController : Controller
    {
        private IReviewRepository repository;
        private UserManager<User> userManager;

        IWebHostEnvironment environment;
        public AdminController(IReviewRepository repo, IWebHostEnvironment env, UserManager<User> usrManager)
        {
            repository = repo;
            environment = env;
            userManager = usrManager;
        }
        [Authorize]
        public ViewResult  Index()
        {
            string Id = userManager.GetUserId(User);
            return View(new Admin
            {
                
                Reviews = repository.Reviews
                .Where(r => r.ReviewAuthorID == Id).ToList(),
                currentUser = userManager.Users.FirstOrDefault(u => u.Id == Id)
            }); 
        }
        
        public ViewResult Edit(int ID) =>
            View(repository.Reviews.FirstOrDefault(r => r.ID == ID));
        
        [HttpPost]
        public IActionResult Edit(Review review)
        {   
            
            if (ModelState.IsValid)
            {
                string Id = userManager.GetUserId(User);
                review.ReviewAuthor = userManager.GetUserName(User);
                review.ReviewAuthorID = Id;
                //review.GradedUser.Add(Id);
                repository.SaveReview(review);
                TempData["message"] = $"{review.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                return View(review);
            }
           
        }
        public ViewResult Create() => View("Edit", new Review());
        public ViewResult CreateUser() => View();
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        public ViewResult LogIn() => View();
        [HttpPost]
        public IActionResult Delete(int ID)
        {
            Review toDelete = repository.DeleteReview(ID);
            if(toDelete != null)
            {
                TempData["message"] = $"{toDelete.Name} was deleted.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult BatchUpload()
        {
            bool uploadSuccessfully = true;
            int count = 0;
            string msg = "",
                    fileName = "",
                    fileExtension = "",
                    filePath = "",
                    fileNewName = "";

            try
            {
                string directoryPath = environment.WebRootPath + "/Content/cover";
                ViewBag.DirPath = directoryPath;
                if (!Directory.Exists(directoryPath))
                { Directory.CreateDirectory(directoryPath); }
                foreach (var f in Request.Form.Files)
                {
                    //IFormFile file = Request.Form.Files[f];
                    if (f.Length > 0)
                    {
                        fileName = f.FileName;
                        fileExtension = Path.GetExtension(fileName);
                        fileNewName = Guid.NewGuid().ToString() + fileExtension;
                        filePath = Path.Combine(directoryPath, fileNewName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            f.CopyToAsync(fileStream);
                        }
                        count++;
                    }
                }
            }
            catch (Exception e)
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
