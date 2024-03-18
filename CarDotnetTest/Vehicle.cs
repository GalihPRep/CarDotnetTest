using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDotnetTest
{
    class Vehicle
    {
        public Vehicle()
        {
        }

        public Vehicle(string? number, string? type, string? color, DateTime checkIn)
        {
            Number = number;
            Type = type; Color = color; CheckIn = checkIn;
        }
        public string? Number { get; set; }
        public string? Type { get; set; }
        public string? Color { get; set; }
        public DateTime? CheckIn { get; set; }
    }
}
