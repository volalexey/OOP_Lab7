using Lab_7;
using System.Globalization;

namespace TestLab_7
{
    [TestClass]
    public class TestProgramStaticMethods
    {
        private CultureInfo testCulture = CultureInfo.InvariantCulture;

        [TestInitialize]
        public void Initialize()
        {
            Program.planets = new List<Planet>();
            Planet.PlanetCount = 0;
            Program.rnd = new System.Random();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Program.planets.Clear();
            Planet.PlanetCount = 0;
        }

        [TestMethod]
        public void AddDefaultPlanet_ShouldAddPlanet()
        {
            // Arrange
            PlanetType type = PlanetType.Terrestrial;
            string name = "TestPlanet";

            // Act
            Program.AddDefaultPlanet(Program.planets, type, name);

            // Assert
            Assert.AreEqual(1, Program.GetPlanetCount());
            Assert.AreEqual(1, Planet.PlanetCount);
            Assert.AreEqual("TestPlanet", Program.planets[0].Name);
        }

        [TestMethod]
        public void AddRandomPlanet_ShouldAddPlanet()
        {
            // Act
            Program.AddRandomPlanet(Program.planets);

            // Assert
            Assert.AreEqual(1, Program.GetPlanetCount());
            Assert.AreEqual(1, Planet.PlanetCount);
            Assert.IsNotNull(Program.planets[0]);
        }


        [TestMethod]
        public void AddPlanetFromString_WithValidString_ShouldAddPlanet()
        {
            // Arrange
            string validData = "Terrestrial|Mars|6.4e23|3.39e6|228e6|false|0";

            // Act
            bool result = Program.AddPlanetFromString(Program.planets, validData, out Planet parsedPlanet, out string error);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, Program.GetPlanetCount());
            Assert.AreEqual(1, Planet.PlanetCount);
            Assert.IsNotNull(parsedPlanet);
            Assert.AreEqual("Mars", parsedPlanet.Name);
            Assert.AreEqual(6.4e23, parsedPlanet.Mass);
            Assert.IsTrue(string.IsNullOrEmpty(error));
        }

        [TestMethod]
        public void AddPlanetFromString_WithInvalidString_ShouldReturnFalse()
        {
            // Arrange
            string invalidData = "Terrestrial|Mars|WRONG|3.39e6|228e6|false|0";

            // Act
            bool result = Program.AddPlanetFromString(Program.planets, invalidData, out Planet parsedPlanet, out string error);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, Program.GetPlanetCount());
            Assert.AreEqual(0, Planet.PlanetCount);
            Assert.IsNull(parsedPlanet);
            Assert.IsFalse(string.IsNullOrEmpty(error));
        }

        [TestMethod]
        public void GetPlanetCount_ShouldReturnCorrectCount()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Mars"));

            // Act
            int count = Program.GetPlanetCount();

            // Assert
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void GetAllPlanets_ShouldReturnListReference()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth"));
            List<Planet> mainList = Program.planets;

            // Act
            List<Planet> resultList = Program.GetAllPlanets(mainList);

            // Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(1, resultList.Count);
            Assert.AreSame(mainList, resultList);
        }

        [TestMethod]
        public void FindPlanetsByName_WhenPlanetExists_ShouldReturnList()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Mars"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "earth"));

            // Act
            var result = Program.FindPlanetsByName(Program.planets, "Earth");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void FindPlanetsByName_WhenPlanetNotExist_ShouldReturnEmptyList()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Mars"));

            // Act
            var result = Program.FindPlanetsByName(Program.planets, "Earth");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FindPlanetsByType_ShouldReturnCorrectPlanets()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth"));
            Program.planets.Add(new Planet(PlanetType.GasGiant, "Jupiter"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Mars"));

            // Act
            var result = Program.FindPlanetsByType(Program.planets, PlanetType.Terrestrial);
            var result2 = Program.FindPlanetsByType(Program.planets, PlanetType.Dwarf);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Earth", result[0].Name);
            Assert.AreEqual("Mars", result[1].Name);
            Assert.AreEqual(0, result2.Count);
        }

        [TestMethod]
        public void FindFirstPlanetByName_ShouldReturnFirstMatch()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Mars"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "earth"));

            // Act
            Planet result = Program.FindFirstPlanetByName(Program.planets, "Earth");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Program.planets[0], result);
        }

        [TestMethod]
        public void DeletePlanetsByName_WhenPlanetExists_ShouldRemoveThem()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Mars"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "EARTH"));
            Planet.PlanetCount = 3;

            // Act
            int removedCount = Program.DeletePlanetsByName(Program.planets, "Earth");
            int finalCount = Program.GetPlanetCount();

            // Assert
            Assert.AreEqual(2, removedCount);
            Assert.AreEqual(1, finalCount);
            Assert.AreEqual("Mars", Program.planets[0].Name);
        }

        [TestMethod]
        public void DeletePlanetsByType_ShouldRemoveCorrectPlanets()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth"));
            Program.planets.Add(new Planet(PlanetType.GasGiant, "Jupiter"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Mars"));

            // Act
            int removedCount = Program.DeletePlanetsByType(Program.planets, PlanetType.Terrestrial);

            // Assert
            Assert.AreEqual(2, removedCount);
            Assert.AreEqual(1, Program.GetPlanetCount());
            Assert.AreEqual("Jupiter", Program.planets[0].Name);
        }

        [TestMethod]
        public void DeletePlanetByIndex_ShouldRemoveCorrectPlanet()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth"));
            Program.planets.Add(new Planet(PlanetType.GasGiant, "Jupiter"));
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Mars"));

            // Act
            Planet removedPlanet = Program.DeletePlanetByIndex(Program.planets, 1);

            // Assert
            Assert.IsNotNull(removedPlanet);
            Assert.AreEqual("Jupiter", removedPlanet.Name);
            Assert.AreEqual(2, Program.GetPlanetCount());
            Assert.AreEqual("Earth", Program.planets[0].Name);
            Assert.AreEqual("Mars", Program.planets[1].Name);
        }

        [TestMethod]
        public void SerializeAsCsv_ShouldReturnCorrectString()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth", 5.97e24, 6.371e6, 150e6, true));
            Program.planets.Add(new Planet(PlanetType.GasGiant, "Jupiter", 1.9e27, 6.9911e7, 778e6, false));
            Program.planets[0].Age = 1;

            // Act
            string csv = Program.SerializeAsCsv(Program.planets);

            // Assert
            Assert.IsNotNull(csv);
            string expectedHeader = "Type|Name|Mass|Radius|DistanceFromSun|HasLife|Age";
            string expectedEarth = "Terrestrial|Earth|5.97E+24|6371000|150000000|True|1";
            string expectedJupiter = "GasGiant|Jupiter|1.9E+27|69911000|778000000|False|0";

            Assert.IsTrue(csv.StartsWith(expectedHeader));
            Assert.IsTrue(csv.Replace(",", ".").Contains(expectedEarth.Replace("E+", "e+"), StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(csv.Replace(",", ".").Contains(expectedJupiter.Replace("E+", "e+"), StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void DeserializeFromCsv_WithValidData_ShouldLoadPlanets()
        {
            // Arrange
            string csvData = "Type|Name|Mass|Radius|DistanceFromSun|HasLife|Age\n" +
                             "Terrestrial|Earth|5.97e24|6.371e6|150e6|true|0\n" +
                             "GasGiant|Jupiter|1.9e27|6.9911e7|778e6|false|5";

            // Act
            List<Planet> loaded = Program.DeserializeFromCsv(csvData);

            // Assert
            Assert.IsNotNull(loaded);
            Assert.AreEqual(2, loaded.Count);
            Assert.AreEqual("Earth", loaded[0].Name);
            Assert.AreEqual(1.9e27, loaded[1].Mass);
            Assert.AreEqual(5, loaded[1].Age);
        }

        [TestMethod]
        public void DeserializeFromCsv_WithInvalidData_ShouldSkipBadLines()
        {
            // Arrange
            string csvData = "Type|Name|Mass|Radius|DistanceFromSun|HasLife|Age\n" +
                             "Terrestrial|Earth|5.97e24|6.371e6|150e6|true|0\n" +
                             "Terrestrial|BadData|WRONG|3.39e6|228e6|false|0\n" +
                             "Terrestrial|Mars|6.4e23|3.39e6|228e6|false|0";

            // Act
            List<Planet> loaded = Program.DeserializeFromCsv(csvData);

            // Assert
            Assert.IsNotNull(loaded);
            Assert.AreEqual(2, loaded.Count);
            Assert.AreEqual("Earth", loaded[0].Name);
            Assert.AreEqual("Mars", loaded[1].Name);
        }

        [TestMethod]
        public void SerializeAsJson_ShouldReturnCorrectString()
        {
            // Arrange
            Program.planets.Add(new Planet(PlanetType.Terrestrial, "Earth", 5.97e24, 6.371e6, 150e6, true));

            // Act
            string json = Program.SerializeAsJson(Program.planets);

            // Assert
            Assert.IsTrue(json.Contains("\"Name\": \"Earth\""));
            Assert.IsFalse(json.Contains("DistanceTraveled"));
        }

        [TestMethod]
        public void DeserializeFromJson_WithValidData_ShouldLoadPlanets()
        {
            // Arrange
            string jsonData = @"[
                {'Name':'Earth','Mass':5.97e24,'Radius':6.371e6,'Type':0,'DistanceFromSun':150e6,'HasLife':true,'Age':0},
                {'Name':'Mars','Mass':6.4e23,'Radius':3.39e6,'Type':0,'DistanceFromSun':228e6,'HasLife':false,'Age':5}
            ]";

            // Act
            List<Planet> loaded = Program.DeserializeFromJson(jsonData);

            // Assert
            Assert.IsNotNull(loaded);
            Assert.AreEqual(2, loaded.Count);
            Assert.AreEqual("Earth", loaded[0].Name);
            Assert.AreEqual(5, loaded[1].Age);
        }

        [TestMethod]
        public void DeserializeFromJson_WithCompletelyInvalidJson_ShouldReturnEmptyList()
        {
            // Arrange
            string jsonData = @"{'this': 'is not an array'}";
            string jsonData2 = @"[ {'Name':'Earth', 'Mass': 123 ]";

            // Act
            List<Planet> loaded = Program.DeserializeFromJson(jsonData);
            List<Planet> loaded2 = Program.DeserializeFromJson(jsonData2);

            // Assert
            Assert.IsNotNull(loaded);
            Assert.AreEqual(0, loaded.Count);
            Assert.IsNotNull(loaded2);
            Assert.AreEqual(0, loaded2.Count);
        }
    }
}