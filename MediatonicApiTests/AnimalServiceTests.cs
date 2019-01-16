using MediatonicApi.Models;
using MediatonicApi.Models.Exceptions;
using MediatonicApi.Models.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    public class AnimalServiceTests
    {
        DbContextOptions<ApiContext> dbOptions;

        [SetUp]
        public void Setup()
        {
            dbOptions = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase("animal_service_tests")
                .Options
            ;
        }

        [Test]
        public void TestAddAnimal()
        {
            Animal animal = new Animal() {
                TypeName = "Some new animal",
                HungerPerSecond = 0.1m,
                SadnessPerSecond = 0.1m
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                service.Add(animal);
            }

            // Make sure the animal was added to the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.Animals.Count());
                Assert.AreNotEqual(0, context.Animals.First().Id);
                Assert.IsTrue(context.Animals.Any(a => a.TypeName == animal.TypeName));
            }

            // Make sure that white space is trimmed
            string newAnimalName = "  New animal type  ";
            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                Animal animal2 = new Animal {
                    TypeName = newAnimalName,
                    HungerPerSecond = 0.1m,
                    SadnessPerSecond = 0.1m
                };

                service.Add(animal2);
            }

            // Make sure the user was added to the DB and auto-increment works
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(2, context.Animals.Count());
                Assert.AreNotEqual(0, context.Animals.Skip(1).First().Id);
                Assert.IsTrue(context.Animals.Any(a => a.TypeName == newAnimalName.Trim()));
            }
        }

        [Test]
        public void TestAddAnimalNoType()
        {
            Animal animal = new Animal() {
                TypeName = "",
                HungerPerSecond = 0.1m,
                SadnessPerSecond = 0.1m
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                // Test with an empty string
                Assert.Catch<System.ArgumentException>(() => service.Add(animal));

                // Test with only whitespace
                animal.TypeName = "    ";
                Assert.Catch<System.ArgumentException>(() => service.Add(animal));

                // Test with type as null
                animal.TypeName = null;
                Assert.Catch<System.ArgumentException>(() => service.Add(animal));
            }

            // Make sure no animals are in the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(0, context.Animals.Count());
            }
        }

        [Test]
        public void TestAddAnimalDuplicate()
        {
            Animal animal = new Animal() {
                TypeName = "Some new animal",
                HungerPerSecond = 0.1m,
                SadnessPerSecond = 0.1m
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Animals.Add(animal);
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                Assert.Catch<DuplicateEntryException>(() => service.Add(animal));

                // Test with whitespace
                animal.TypeName = "   Some new animal ";
                Assert.Catch<DuplicateEntryException>(() => service.Add(animal));

                // Test with different casing
                animal.TypeName = "some New Animal";
                Assert.Catch<DuplicateEntryException>(() => service.Add(animal));
            }

            // Make sure no animals are in the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.Animals.Count());
            }
        }

        [Test]
        public void TestAnimalInvalidHunger()
        {
            Animal animal = new Animal() {
                TypeName = "Some new animal",
                HungerPerSecond = 0,
                SadnessPerSecond = 0.1m
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                Assert.Catch<System.ArgumentException>(() => service.Add(animal));

                // Test with a value lower than zero
                animal.HungerPerSecond = -0.1m;
                Assert.Catch<System.ArgumentException>(() => service.Add(animal));
            }

            // Make sure no animals are in the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(0, context.Animals.Count());
            }
        }

        [Test]
        public void TestAnimalInvalidSadness()
        {
            Animal animal = new Animal() {
                TypeName = "Some new animal",
                HungerPerSecond = 0.1m,
                SadnessPerSecond = 0
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                Assert.Catch<System.ArgumentException>(() => service.Add(animal));

                // Test with a value lower than zero
                animal.SadnessPerSecond = -0.1m;
                Assert.Catch<System.ArgumentException>(() => service.Add(animal));
            }

            // Make sure no animals are in the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(0, context.Animals.Count());
            }
        }

        [Test]
        public void TestFindOne()
        {
            string animalType = "Some new animal";
            Animal animal = new Animal() {
                TypeName = animalType,
                HungerPerSecond = 0.1m,
                SadnessPerSecond = 0.1m
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Animals.Add(animal);
                context.Animals.Add(new Animal() { TypeName = "Another random animal" });
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                Assert.AreNotEqual(null, service.FindOne(animal.Id));
                Assert.AreEqual(animalType, service.FindOne(animal.Id).TypeName);
            }
        }

        [Test]
        public void TestFindOneInvalid()
        {
            Animal animal = new Animal() {
                TypeName = "Some new animal",
                HungerPerSecond = 0.1m,
                SadnessPerSecond = 0.1m
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Animals.Add(animal);
                context.Animals.Add(new Animal() { TypeName = "Another random animal" });
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                Assert.IsNull(service.FindOne(animal.Id + 2));
            }
        }


        [Test]
        public void TestFindAll()
        {
            Animal animal = new Animal() {
                TypeName = "Some animal",
                HungerPerSecond = 0.1m,
                SadnessPerSecond = 0.1m
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Animals.Add(animal);
                context.Animals.Add(new Animal() { TypeName = "Another random animal" });
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                AnimalService service = new AnimalService(context);

                Assert.AreEqual(2, service.FindAll().Count());
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Remove everything in the DB - ready to test again
            using (ApiContext context = new ApiContext(dbOptions)) {
                foreach (User u in context.Users) {
                    context.Remove(u);
                }

                foreach (Animal a in context.Animals) {
                    context.Remove(a);
                }

                foreach (UserAnimal ua in context.UserAnimals) {
                    context.Remove(ua);
                }

                context.SaveChanges();
            }
        }
    }
}
