using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalSystem.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string? Brand { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "Passing year is required")]
        public int PassingYear { get; set; }

        [Required(ErrorMessage = "Car number is required")]
        public string? CarNumber { get; set; }

        [Required(ErrorMessage = "Engine type is required")]
        public string? Engine { get; set; }

        [Required(ErrorMessage = "Fuel type is required")]
        public string? FuelType { get; set; }

        [Required(ErrorMessage = "Seating capacity is required")]
        public int SeatingCapacity { get; set; }

        // Додано поле для ціни за день
        [Required(ErrorMessage = "Price per day is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price per day must be a positive number")]
        public decimal PricePerDay { get; set; }

        [NotMapped]
        public IFormFile? CarImage { get; set; } // для завантаження зображення

        public string? ImagePath { get; set; } // шлях до збереженого зображення
    }
}
