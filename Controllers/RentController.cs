using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarRentalSystem.Controllers
{
    public class RentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Дія для відображення списку оренд
        public IActionResult Index()
        {
            var rents = _context.Rentals.Include(r => r.Car).ToList(); // Додаємо тільки автомобіль
            return View(rents);
        }

        public IActionResult Create(int carId)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                return NotFound();
            }

            var rent = new Rent
            {
                CarId = car.Id,
                Car = car  // Додаємо зв'язок з автомобілем
            };

            return View(rent); // Передаємо модель з автомобілем
        }


        // Дія для обробки форми створення оренди
        [HttpPost]
        public IActionResult Create(Rent rent)
        {
            if (ModelState.IsValid)
            {
                // Перевірка наявності автомобіля в оренді на вибрані дати
                var existingRentals = _context.Rentals
                    .Where(r => r.CarId == rent.CarId &&
                                (rent.StartDate < r.EndDate && rent.EndDate > r.StartDate))
                    .Any();

                if (existingRentals)
                {
                    ModelState.AddModelError("", "Цей автомобіль вже заброньовано на ці дати.");
                    return View(rent);  // Повертається на ту саму сторінку з помилкою
                }

                // Додаємо нову оренду в базу даних
                _context.Rentals.Add(rent);
                _context.SaveChanges();

                // Перенаправлення на сторінку списку оренд
                return RedirectToAction("Index");
            }

            // Якщо модель не валідна, повертаємося на ту ж сторінку
            return View(rent);
        }
    }
}
