using CarDotnetTest;

internal class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            var input = Console.ReadLine();

            try
            {
                if (input.Contains("create_parking_lot")) CreateParkingLot(input);
                else if (input.Contains("park")) Park(input);
                else if (input.Contains("leave")) Leave(input);
                else if (input.Contains("status")) Status();
                else if (input.Contains("type_of_vehicles")) TypeOfVehicles(input);
                else if (input.Contains("registration_numbers_for_vehicles_with_"))
                    RegistrationNumbersForVehiclesWith(input);
                else if (input.Contains("slot_numbers_for_vehicles_with_colour"))
                    SlotNumbersForVehiclesWithColor(input);
                else if (input.Contains("slot_number_for_registration_number"))
                    SlotNumbersForRegistrationNumber(input);
                else if (input.Contains("report")) Report();
                else if (input == "exit") break;
            }
            catch
            {
                Console.WriteLine("Invalid input!");
            }
            Console.WriteLine();
        }
    }

    public static List<Vehicle?> _lot = new()
    {
        new Vehicle("B-1234-XYZ", "mobil", "putih", DateTime.Now),
        new Vehicle("B-9999-XYZ", "motor", "putih", DateTime.Now),
        new Vehicle("D-0001-HIJ", "mobil", "hitam", DateTime.Now),
        new Vehicle("B-7777-DEF", "mobil", "merah", DateTime.Now),
        new Vehicle("B-2701-XXX", "mobil", "biru", DateTime.Now),
        new Vehicle("B-3141-ZZZ", "motor", "hitam", DateTime.Now),
    };



    static void CreateParkingLot(string query)
    {
        _lot.Clear();
        var slot = int.Parse(query.Split(" ")[1]);
        for (int i = 0; i < slot; i++) _lot.Add(null);
        Console.WriteLine($"Created a parking lot with {slot} slot.");
    }
    static void Park(string query)
    {
        var slot = _lot.IndexOf(null);
        if (slot != -1)
        {
            var vehicle = query.Split(" ");
            if (
                vehicle[3].ToLower() == "mobil"
                || vehicle[3].ToLower() == "motor"
            )
            {
                _lot[slot] = new Vehicle()
                {
                    Number = vehicle[1],
                    Type = vehicle[3],
                    Color = vehicle[2],
                    CheckIn = DateTime.Now
                };
                Console.WriteLine($"Allocated slot number: {slot + 1}");
            }
            else Console.WriteLine("Kendaraan yang boleh masuk hanya Mobil Kecil dan Motor.");
           
        }
        else Console.WriteLine("Sorry, parking lot is full");
    }
    static void Leave(string query)
    {
        var slot = int.Parse(query.Split(" ")[1]);
        var checkIn = _lot[slot - 1]?.CheckIn;
        _lot[slot - 1] = null;
        Console.WriteLine($"Slot number {slot} is free");
        Console.WriteLine($"Fee: Rp {Math.Ceiling(
            (decimal)DateTime.Now.Subtract(checkIn ?? DateTime.Now).TotalHours
        ) * 2000}");
    }
    static void Status()
    {
        var border = string.Join("", Enumerable.Repeat("-", 12));
        Console.WriteLine(string.Format(
            "| {0} | {1} | {2} | {3} |\n|-{4}-|-{5}-|-{6}-|-{7}-|",
            "Slot".PadRight(4),
            "No.".PadRight(12),
            "Type".PadRight(12),
            "Registration No. Colour".PadRight(24),
            string.Join("", Enumerable.Repeat("-", 4)),
            border,
            border,
            string.Join("", Enumerable.Repeat(border, 2))
        ));
        for (var i = 0; i < _lot.Count; i++)
        {
            if (_lot[i] != null) Console.WriteLine(string.Format(
            "| {0} | {1} | {2} | {3} |",
            (i + 1).ToString().PadRight(4),
            _lot[i]?.Number?.PadRight(12),
            _lot[i]?.Type?.PadRight(12),
            _lot[i]?.Color?.PadRight(24)
            ));
        }
    }
    public static void TypeOfVehicles(string query) =>
        Console.WriteLine(_lot.Where(
            n => n?.Type?.ToLower() == query.Split(" ")[1].ToLower()
        ).Count());
    public static void RegistrationNumbersForVehiclesWith(string query)
    {
        if (query.Contains("odd") || query.Contains("ood"))
            Console.WriteLine(string.Join(", ", _lot.Where(n =>
                n != null && int.Parse(n?.Number?.Split("-")[1]) % 2 != 0
            ).Select(n => n?.Number)));
        else if (query.Contains("even"))
            Console.WriteLine(string.Join(", ", _lot.Where(n =>
                n != null && int.Parse(n?.Number?.Split("-")[1]) % 2 == 0
            ).Select(n => n?.Number)));
        else if (query.Contains("colour") || query.Contains("color"))
            Console.WriteLine(string.Join(", ", _lot.Where(n =>
                n != null && n?.Color?.ToLower() == query.Split(" ")[1].ToLower()
            ).Select(n => n?.Number)));
    }
    public static void SlotNumbersForVehiclesWithColor(string query) =>
        Console.WriteLine(string.Join(
            ", ",
            Enumerable
                .Range(0, _lot.Count)
                .ToDictionary(i => i, i => _lot[i])
                .Where(n => n.Value?.Color?.ToLower() == query.Split(" ")[1].ToLower())
                .Select(n => n.Key + 1)
        ));
    public static void SlotNumbersForRegistrationNumber(string query)
    {
        var index = _lot.FindIndex(n => n?.Number == query.Split(" ")[1]) + 1;
        if (index == 0) Console.WriteLine("Not found");
        else Console.WriteLine(index);
    }
    public static void Report()
    {
        Console.WriteLine($"Slot terisi: {_lot.Where(n => n != null).Count()}");
        Console.WriteLine($"Slot tersedia: {_lot.Where(n => n == null).Count()}");

        Console.WriteLine($"Kendaraan ganjil: {
            _lot.Where(n => 
                n != null && int.Parse(n?.Number?.Split("-")[1]) % 2 != 0
            ).Count()
        }");
        Console.WriteLine($"Kendaraan genap: {
            _lot.Where(n =>
                n != null && int.Parse(n?.Number?.Split("-")[1]) % 2 == 0
            ).Count()
        }");

        var types = _lot.Select(n => n?.Type).Distinct().ToList();
        Console.WriteLine(
           string.Join("\n", types.Select(n => 
               $"Kendaraan jenis {n}: {
                   _lot.Where(o => o?.Type == n).Count()
               }"
           ))
        );
        var colors = _lot.Select(n => n?.Color).Distinct().ToList();
        Console.WriteLine(
           string.Join("\n", colors.Select(n =>
               $"Kendaraan warna {n}: {_lot.Where(o => o?.Color == n).Count()}"
           ))
        );
    }

}