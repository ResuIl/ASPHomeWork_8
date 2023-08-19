using ASPHomeWork_8.Models.ViewModels;
using ASPHomeWork_8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPHomeWork_8.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> userManager;
    private readonly SignInManager<AppUser> signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public IActionResult Login(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            AppUser user = new()
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Year = model.Year
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                string confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                //Console.WriteLine(confirmToken);
                var url = Url.Action(nameof(ConfirmEmail), "Account", new { confirmToken, email = user.Email }, Request.Scheme);
                Console.WriteLine(url);
                //emailService.Send();
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null)
        {
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                if (await userManager.IsEmailConfirmedAsync(user))
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(returnUrl))
                            return Redirect(returnUrl);
                        return Redirect("/");
                    }
                    else if (result.IsLockedOut)
                        ModelState.AddModelError("All", "Lockout");
                }
                else
                    ModelState.AddModelError("All", "Mail has not confired yet!!");

            }
            else
                ModelState.AddModelError("login", "Incorrect username or password");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
