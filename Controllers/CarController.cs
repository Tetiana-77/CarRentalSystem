using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalSystem.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Car
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car, IFormFile carImage)
        {
            if (ModelState.IsValid)
            {
                // Перевірка на правильність ціни за день
                if (car.PricePerDay <= 0)
                {
                    ModelState.AddModelError("PricePerDay", "Ціна за день повинна бути більшою за 0.");
                    return View(car);
                }

                if (carImage != null && carImage.Length > 0)
                {
                    // Генерація унікального імені для файлу
                    var fileName = Path.GetFileNameWithoutExtension(carImage.FileName);
                    var extension = Path.GetExtension(carImage.FileName);
                    var uniqueFileName = fileName + "_" + Guid.NewGuid() + extension;

                    // Шлях для збереження файлу
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await carImage.CopyToAsync(stream);
                    }

                    car.ImagePath = "/images/" + uniqueFileName;
                }

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(car);
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,PassingYear,CarNumber,Engine,FuelType,SeatingCapacity,PricePerDay,ImagePath")] Car car, IFormFile carImage)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Перевірка на правильність ціни за день
                    if (car.PricePerDay <= 0)
                    {
                        ModelState.AddModelError("PricePerDay", "Ціна за день повинна бути більшою за 0.");
                        return View(car);
                    }

                    if (carImage != null && carImage.Length > 0)
                    {
                        // Якщо зображення було змінене, видаляємо старе зображення
                        if (!string.IsNullOrEmpty(car.ImagePath))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, car.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Генерація унікального імені для файлу
                        var fileName = Path.GetFileNameWithoutExtension(carImage.FileName);
                        var extension = Path.GetExtension(carImage.FileName);
                        var uniqueFileName = fileName + "_" + Guid.NewGuid() + extension;

                        // Шлях для збереження файлу
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await carImage.CopyToAsync(stream);
                        }

                        car.ImagePath = "/images/" + uniqueFileName;
                    }

                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(car);
        }

        // GET: Car/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            // Видалення зображення з сервера
            if (!string.IsNullOrEmpty(car.ImagePath))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, car.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        // Додатковий екшн для відображення деталей автомобіля
        public IActionResult Details(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            var rentalViewModel = new RentalViewModel
            {
                Car = car,
                Drivers = _context.Drivers.ToList()
            };

            return View(rentalViewModel);
        }

        

    }
}
