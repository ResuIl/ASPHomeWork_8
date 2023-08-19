using ASPHomeWork_8.Data;
using ASPHomeWork_8.Models;
using ASPHomeWork_8.Services.Claims;
using ASPHomeWork_8.Services.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("default")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = true;

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<EcommerceDbContext>().AddDefaultTokenProviders();


builder.Services.AddAuthorization(
    opts =>
    {
        opts.AddPolicy("Email", policy => policy.Requirements.Add(new EmailRequirement("admin.com")));
    }
);

builder.Services.AddSingleton<IAuthorizationHandler, EmailHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapAreaControllerRoute(
        name: "foradminarea",
        areaName: "Admin",
        pattern: "foradmin/{controller=Home}/{action=Index}"
        );
    //endpoint.MapControllerRoute(
    //    name: "foradmin",
    //    pattern: "{area}/{controller=Home}/{action=Index}"
    //    );
    endpoint.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

var container = app.Services.CreateScope();
var userManager = container.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
var roleManager = container.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
if (!await roleManager.RoleExistsAsync("Admin"))
{
    var result = await roleManager.CreateAsync(new IdentityRole("Admin"));
    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
}

var user = await userManager.FindByEmailAsync("admin@admin.com");
if (user is null)
{
    user = new AppUser
    {
        UserName = "admin@admin.com",
        Email = "admin@admin.com",
        FullName = "Admin",
        Year = 2023,
        EmailConfirmed = true
    };
    var result = await userManager.CreateAsync(user, "Admin12!");
    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
    result = await userManager.AddToRoleAsync(user, "Admin");
}

app.MapRazorPages();

app.Run();
