using ASPHomeWork_8.Models.ViewModels;
using ASPHomeWork_8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPHomeWork_8.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "Email")]
//[Authorize(Roles = "Admin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Products()
    {
        return View();
    }

    public IActionResult AddProduct()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(ProductViewModel ProductVM)
    {
        if (ModelState.IsValid)
        {
            products.Add(ProductVM.Product!);
            return RedirectToAction("Get");
        }
        return View();
    }
}
