using Microsoft.AspNetCore.Identity;

namespace CarRentalSystem.Models
{
    // Наслідується від IdentityUser для збереження стандартних даних користувача (логін, email і т.д.)
    public class ApplicationUser : IdentityUser
    {
        // Додаткові властивості для користувача (наприклад, повне ім'я)
        public string FullName { get; set; }
    }
}
