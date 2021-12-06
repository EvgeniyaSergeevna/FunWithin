using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FunWithin.Models;

namespace FunWithin.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IReviewRepository repository;
        public NavigationMenuViewComponent(IReviewRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedType = RouteData?.Values["type"];
            return View(repository.Reviews
                .Select(r => r.Type)
                .Distinct()
                .OrderBy(r => r));
        }
    }
}
