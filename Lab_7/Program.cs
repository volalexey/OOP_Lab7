using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Lab_7
{
    public class Program
    {
        public static List<Planet> planets = new List<Planet>();
        public static Random rnd = new Random();

        private const string JsonFilePath = "planets.json";
        private const string CsvFilePath = "planets.csv";

        static void Main(string[] args)
        {
            int menu = 0;

            do
            {
                Console.WriteLine("\n1. Add object");
                Console.WriteLine("2. View all objects");
                Console.WriteLine("3. Find object");
                Console.WriteLine("4. Demonstrate behavior");
                Console.WriteLine("5. Delete object");
                Console.WriteLine("6. Demonstrate static methods");
                Console.WriteLine("7. Save planets to file");
                Console.WriteLine("8. Load planets from file");
                Console.WriteLine("9. Clear planets");
                Console.WriteLine("0. Exit program\n");

                menu = InputHelper.GetIntValue("Choose your option: ", "Invalid choice.", 0, 9);

                switch (menu)
                {
                    case 1: AddObjectMenu(); break;
                    case 2: ShowPlanetsTable(); break;
                    case 3: FindObjectMenu(); break;
                    case 4: DemonstrateBehaviorMenu(); break;
                    case 5: DeleteObjectMenu(); break;
                    case 6: DemonstrateStaticMethodsMenu(); break;
                    case 7: SavePlanetsMenu(); break;
                    case 8: LoadPlanetsMenu(); break;
                    case 9: planets.Clear(); Planet.PlanetCount = 0; Console.WriteLine("List cleared"); break;
                    case 0: Console.WriteLine("Exiting. Goodbye!"); break;
                }

            } while (menu != 0);
        }

        //menu methods
        private static void DemonstrateStaticMethodsMenu()
        {
            string galaxy = InputHelper.GetStringValue("Input galaxy: ", "invalid", s => s.Length != 0);
            Planet.Galaxy = galaxy;
            Console.WriteLine("\n--- Demonstrating static methods ---");
            Console.WriteLine(Planet.ShowUniverseInfo());

            if (GetPlanetCount() == 0)
            {
                Console.WriteLine("No planets to display.");
                return;
            }

            Console.WriteLine("\nPlanets (ToString format for parsing):");
            foreach (var p in planets)
            {
                Console.WriteLine(p.ToString());
            }
        }

        private static void AddObjectMenu()
        {
            Console.WriteLine("1. Add manually");
            Console.WriteLine("2. Add random planet");
            Console.WriteLine("3. Generate default (only type and name)");
            Console.WriteLine("4. Add Parse (string input)");

            int choice = InputHelper.GetIntValue("Choose: ", "Invalid.", 1, 4);

            if (choice == 1)
            {
                var planet = new Planet();

while (true)
                {
                    try
                    {
                        Console.Write("Choose planet type (0 - Terrestrial, 1 - GasGiant, 2 - IceGiant, 3 - Dwarf): ");
                        int typeInt = int.Parse(Console.ReadLine()!);
                        planet.Type = (PlanetType)typeInt;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Enter name: ");
                        planet.Name = Console.ReadLine();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Enter mass (kg): ");
                        planet.Mass = double.Parse(Console.ReadLine()!);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Enter radius (m): ");
                        planet.Radius = double.Parse(Console.ReadLine()!);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Enter distance from Sun (mln km): ");
                        planet.DistanceFromSun = double.Parse(Console.ReadLine()!);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Has life? (y/n): ");
                        string input = Console.ReadLine()!.ToLower();
                        if (input == "y" || input == "yes") planet.HasLife = true;
                        else if (input == "n" || input == "no") planet.HasLife = false;
                        else throw new ArgumentException("Enter Y or N.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                planets.Add(planet);
                Console.WriteLine("Planet added successfully!");
            }
            else if (choice == 2)
            {
                Planet p = AddRandomPlanet(planets);
                Console.WriteLine($"Random planet generated: {p.Name}");
            }
            else if (choice == 3)
            {
                int typeInt = InputHelper.GetIntValue("Choose planet type (0 - Terrestrial, 1 - GasGiant, 2 - IceGiant, 3 - Dwarf): ", "Invalid choice.", 0, 3);
                PlanetType type = (PlanetType)typeInt;
                string name = InputHelper.GetStringValue("Input name (1-30): ", "Invalid input.", s => s.Length > 0 && s.Length < 31);

                AddDefaultPlanet(planets, type, name);
                Console.WriteLine("Planet added successfully!");
            }
            else if (choice == 4)
            {
                Console.WriteLine("Enter planet data in format: type|name|mass|radius|distance|hasLife|age");
                string input = Console.ReadLine()!;

                if (AddPlanetFromString(planets, input, out Planet parsedPlanet, out string error))
                {
                    Console.WriteLine("Planet added successfully!");
                }
                else
                {
                    Console.WriteLine(error);
                }
            }
        }

        private static void ShowPlanetsTable()
        {
            if (GetPlanetCount() == 0)
            {
                Console.WriteLine("No planets available.");
                return;
            }
            Console.WriteLine($"{Planet.PlanetCount} planets");
            Console.WriteLine($"\n{"#",3} {"Name",-12} | {"Type",-12} | {"Mass (kg)",12} | {"Radius (m)",12} | {"Dist (mln km)",12} | {"Life",-3} | {"Age",5} | {"Speed (km/s)",12} | {"Traveled (km)",12}\n");

            var allPlanets = GetAllPlanets(planets);

            for (int i = 0; i < allPlanets.Count; i++)
            {
                Console.WriteLine($"{i + 1,3} {allPlanets[i].ToTableString()}");
            }
        }

        private static void FindObjectMenu()
        {
            if (GetPlanetCount() == 0) { Console.WriteLine("No planets."); return; }

            int choice = InputHelper.GetIntValue("Find by: 1 - Name, 2 - Type: ", "Invalid.", 1, 2);
            List<Planet> found;

            if (choice == 1)
            {
                Console.Write("Enter name: ");
                string name = Console.ReadLine()!;
                found = FindPlanetsByName(planets, name);
            }
            else
            {
                int typeInt = InputHelper.GetIntValue("Choose type (0-3): ", "Invalid.", 0, 3);
                found = FindPlanetsByType(planets, (PlanetType)typeInt);
            }

            if (found.Count == 0) Console.WriteLine("No matches.");
            else
            {
                foreach (var p in found)
                {
                    Console.WriteLine(p.ToTableString());
                }
            }
        }

        private static void DemonstrateBehaviorMenu()
        {
            if (GetPlanetCount() == 0) { Console.WriteLine("No planets."); return; }

            Console.WriteLine("1. Show gravity");
            Console.WriteLine("2. Make year pass");
            Console.WriteLine("3. Toggle life");

            int choice = InputHelper.GetIntValue("Choose: ", "Invalid.", 1, 3);

            if (choice == 1)
            {
                foreach (var p in planets) { Console.WriteLine($"{p.Name} gravity: {p.GetGravity()} m/s^2"); }
            }
            else if (choice == 2)
            {
                foreach (var p in planets) { Console.WriteLine(p.MakeYearPass()); }
                int years = InputHelper.GetIntValue("Input years: ", "Invalid input.", 1, int.MaxValue);
                foreach (var p in planets) { Console.WriteLine(p.MakeYearPass(years)); }
            }
            else if (choice == 3)
            {
                Console.Write("Enter planet name: ");
                string name = Console.ReadLine();

                Planet planet = FindFirstPlanetByName(planets, name);
                if (planet == null) { Console.WriteLine("Planet not found."); return; }

                bool create = InputHelper.GetYesNo("Do you want to create life? (y = create / n = destroy): ");
                Console.WriteLine(create ? planet.BirthLife() : planet.DestroyLife());
            }
        }

        private static void DeleteObjectMenu()
        {
            if (GetPlanetCount() == 0) { Console.WriteLine("No planets."); return; }

            Console.WriteLine("Delete by: 1 - Name, 2 - Type, 3 - Number in list");
            int choice = InputHelper.GetIntValue("Choose: ", "Invalid.", 1, 3);

            if (choice == 1)
            {
                Console.Write("Enter name: ");
                string name = Console.ReadLine()!;

                int removed = DeletePlanetsByName(planets, name);

                for (int i = 0; i < removed; i++) { Planet.PlanetDestroyed(); }
                Console.WriteLine(removed == 0 ? "No planet found." : $"{removed} planet(s) deleted.");
            }
            else if (choice == 2)
            {
                int typeInt = InputHelper.GetIntValue("Choose type (0-3): ", "Invalid.", 0, 3);

                int removed = DeletePlanetsByType(planets, (PlanetType)typeInt);

                for (int i = 0; i < removed; i++) { Planet.PlanetDestroyed(); }
                Console.WriteLine(removed == 0 ? "No planets of this type found." : $"{removed} planet(s) deleted.");
            }
            else
            {
                ShowPlanetsTable();
                int number = InputHelper.GetIntValue("Enter planet number to delete: ", "Invalid.", 1, planets.Count);

                Planet removed = DeletePlanetByIndex(planets, number - 1);

                Planet.PlanetDestroyed();
                Console.WriteLine($"Planet {removed.Name} deleted.");
            }
        }

        private static void SavePlanetsMenu()
        {
            Console.WriteLine("Choose format to save:");
            Console.WriteLine("1. JSON");
            Console.WriteLine("2. CSV");
            int choice = InputHelper.GetIntValue("Choose: ", "Invalid.", 1, 2);
            string filePath = (choice == 1) ? JsonFilePath : CsvFilePath;

            try
            {
                if (choice == 1)
                {
                    string json = SerializeAsJson(planets);
                    File.WriteAllText(filePath, json);
                }
                else
                {
                    string csv = SerializeAsCsv(planets);
                    File.WriteAllText(filePath, csv);
                }
                Console.WriteLine($"Successfully saved {planets.Count} planets to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        private static void LoadPlanetsMenu()
        {
            Console.WriteLine("Choose format to load:");
            Console.WriteLine("1. JSON");
            Console.WriteLine("2. CSV");
            int choice = InputHelper.GetIntValue("Choose: ", "Invalid.", 1, 2);
            string filePath = (choice == 1) ? JsonFilePath : CsvFilePath;

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found ({filePath})");
                return;
            }

            try
            {
                List<Planet> loadedPlanets;
                if (choice == 1)
                {
                    string json = File.ReadAllText(filePath);
                    loadedPlanets = DeserializeFromJson(json);
                }
                else
                {
                    string csv = File.ReadAllText(filePath);
                    loadedPlanets = DeserializeFromCsv(csv);
                }

                planets.AddRange(loadedPlanets);
                Planet.PlanetCount = planets.Count;
                if (planets.Count == 0)
                {
                    Console.WriteLine("Loaded 0 planets.");
                }
                else
                {
                    Console.WriteLine($"Successfully loaded {loadedPlanets.Count} planets from {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }
        //clear methods
        public static int GetPlanetCount()
        {
             return planets.Count;
        }

        public static List<Planet> GetAllPlanets(List<Planet> planetList)
        {
            return planetList;
        }

        public static Planet AddRandomPlanet(List<Planet> planetList)
        {
            Planet p = new Planet(rnd);
            planetList.Add(p);
            return p;
        }

        public static void AddDefaultPlanet(List<Planet> planetList, PlanetType type, string name)
        {
            planetList.Add(new Planet(type, name));
        }

        public static bool AddPlanetFromString(List<Planet> planetList, string input, out Planet parsedPlanet, out string error)
        {
            if (Planet.TryParse(input, out parsedPlanet, out error))
            {
                planetList.Add(parsedPlanet);
                return true;
            }
            return false;
        }

        public static List<Planet> FindPlanetsByName(List<Planet> planetList, string name)
        {
            return planetList.FindAll(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static List<Planet> FindPlanetsByType(List<Planet> planetList, PlanetType type)
        {
            return planetList.FindAll(p => p.Type == type);
        }

        public static Planet FindFirstPlanetByName(List<Planet> planetList, string name)
        {
            return planetList.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))!;
        }

        public static int DeletePlanetsByName(List<Planet> planetList, string name)
        {
            int removed = planetList.RemoveAll(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return removed;
        }

        public static int DeletePlanetsByType(List<Planet> planetList, PlanetType type)
        {
            int removed = planetList.RemoveAll(p => p.Type == type);
            return removed;
        }

        public static Planet DeletePlanetByIndex(List<Planet> planetList, int index)
        {
            Planet removed = planetList[index];
            planetList.RemoveAt(index);
            return removed;
        }

        public static string SerializeAsJson(List<Planet> planetsToSerialize)
        {
            return JsonConvert.SerializeObject(planetsToSerialize, Formatting.Indented);
        }

        public static List<Planet> DeserializeFromJson(string jsonContent)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Planet>>(jsonContent) ?? new List<Planet>();
            }
            catch (JsonException)
            {
                return new List<Planet>();
            }
        }

        public static string SerializeAsCsv(List<Planet> planetsToSerialize)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Type|Name|Mass|Radius|DistanceFromSun|HasLife|Age");

            foreach (var planet in planetsToSerialize)
            {
                string line = string.Join("|",
                    planet.Type.ToString(),
                    planet.Name,
                    planet.Mass.ToString(CultureInfo.InvariantCulture),
                    planet.Radius.ToString(CultureInfo.InvariantCulture),
                    planet.DistanceFromSun.ToString(CultureInfo.InvariantCulture),
                    planet.HasLife.ToString(),
                    planet.Age.ToString()
                );
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        public static List<Planet> DeserializeFromCsv(string csvContent)
        {
            var newPlanetList = new List<Planet>();
            var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                if (Planet.TryParse(lines[i], out Planet validPlanet, out string error))
                {
                    newPlanetList.Add(validPlanet);
                }
                else
                {
                    continue;
                }
            }
            return newPlanetList;
        }
    }
}