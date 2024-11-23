using System;
using System.Collections.Generic;
using CarRentalSystem.Models;

namespace CarRentalSystem.Models
{
    public class RentalViewModel
    {
        public Rent Rent { get; set; }
        public Car Car { get; set; }
        public List<Driver> Drivers { get; set; }
    }
}
