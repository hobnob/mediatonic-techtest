using MediatonicApi.Models;
using NUnit.Framework;
using System.Threading;

namespace Tests
{
    public class UserAnimalTests
    {
        private Animal testAnimal;

        [SetUp]
        public void Setup()
        {
            testAnimal = new Animal() {
                TypeName = "Test Animal",
                HungerPerSecond = 0.5,
                SadnessPerSecond = 0.25,
                MinHappiness = -1,
                MaxHappiness = 1,
                MaxHunger = 1
            };
        }

        [Test]
        public void TestHungerIncreases()
        {
            UserAnimal userAnimal = new UserAnimal() {
                Animal = testAnimal
            };

            // Make sure that 1 second increases Hunger by one unit (in this case 0.5)
            Thread.Sleep(1000);
            Assert.AreEqual(0.5, userAnimal.Hunger);

            // Make sure half a second doesn't increase Hunger by another half unit
            Thread.Sleep(500);
            Assert.AreEqual(0.5, userAnimal.Hunger);

            // Make sure half a second more increase Hunger by another unit
            Thread.Sleep(500);
            Assert.AreEqual(1, userAnimal.Hunger);

            // Make sure that hunger doesn't go over the threshold
            Thread.Sleep(1000);
            Assert.AreEqual(1, userAnimal.Hunger);
        }

        [Test]
        public void TestHappinessDecrease()
        {
            UserAnimal userAnimal = new UserAnimal() {
                Animal = testAnimal
            };

            // Make sure that 1 second decreases Happiness by one unit (in this case 0.5)
            Thread.Sleep(1000);
            Assert.AreEqual(-0.25, userAnimal.Happiness);

            // Make sure half a second doesn't decrease Happiness by another half unit
            Thread.Sleep(500);
            Assert.AreEqual(-0.25, userAnimal.Happiness);

            // Make sure half a second more decrease Happiness by another unit
            Thread.Sleep(500);
            Assert.AreEqual(-0.5, userAnimal.Happiness);

            // Make sure that happiness doesn't go below the threshold
            Thread.Sleep(4000);
            Assert.AreEqual(-1, userAnimal.Happiness);
        }
    }
}