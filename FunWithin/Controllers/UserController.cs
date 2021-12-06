using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace FunWithin.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        public ViewResult Index() => View(new Dictionary<string, object>
        { ["Placeholder"] = "Placeholder" });
    }
}
