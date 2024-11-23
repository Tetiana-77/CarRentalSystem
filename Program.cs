using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Додавання контролерів (необхідно для підтримки MVC або API)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Додавання контексту бази даних
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Додавання стандартного Identity (без кастомних користувачів)
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
var app = builder.Build();

// Налаштування обробки запитів
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Увімкнення HTTP Strict Transport Security (HSTS) для захисту
}

app.UseHttpsRedirection(); // Переадресація на HTTPS
app.UseStaticFiles(); // Статичні файли (CSS, JS, зображення)

app.UseRouting();  // Налаштування маршрутизації

app.UseAuthentication();  // Додати автентифікацію
app.UseAuthorization();   // Додати авторизацію

// Налаштування маршрутів для контролерів
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();  // Для підтримки Razor Pages, якщо вони є

app.Run();
