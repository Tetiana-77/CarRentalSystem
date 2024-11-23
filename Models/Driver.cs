using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalSystem.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone(ErrorMessage = "Invalid mobile number")]
        public string? MobileNo { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Experience must be a positive number")]
        public int Experience { get; set; }

    
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile? DriverImage { get; set; } // для завантаження зображення водія
    }
}

