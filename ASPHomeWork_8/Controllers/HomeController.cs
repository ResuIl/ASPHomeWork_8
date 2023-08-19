﻿using ASPHomeWork_8.Data;
using ASPHomeWork_8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using X.PagedList;

namespace ASPHomeWork_8.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly EcommerceDbContext context;
    public HomeController(ILogger<HomeController> logger, EcommerceDbContext context)
    {
        _logger = logger;
        this.context = context;
    }
    public IActionResult Index(int page = 1)
    {

        return View(context.Products.ToPagedList(page, 3));
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}