using Microsoft.AspNetCore.Mvc;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using CarRentalSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Controllers
{
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DriverController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Driver/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver, IFormFile driverImage)
        {
            if (ModelState.IsValid)
            {
                if (driverImage != null && driverImage.Length > 0)
                {
                    // Збереження зображення на сервері
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", driverImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await driverImage.CopyToAsync(stream);
                    }
                    driver.ImagePath = "/images/" + driverImage.FileName; // Збереження шляху до зображення
                }

                _context.Drivers.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Перехід на сторінку зі списком автомобілів
            }
            return View(driver);
        }
        // GET: Driver/Index
        public async Task<IActionResult> Index()
        {
            var drivers = await _context.Drivers.ToListAsync();
            return View(drivers);
        }
    }
}
