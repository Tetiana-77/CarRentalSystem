using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Models
{
    public class Rent
    {
        public int Id { get; set; }

        // Властивість для зв'язку з автомобілем
        public int CarId { get; set; }
        public Car Car { get; set; }

        // Дати оренди
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // Ціна оренди
        [Required]
        public decimal Price { get; set; }
    }
}
