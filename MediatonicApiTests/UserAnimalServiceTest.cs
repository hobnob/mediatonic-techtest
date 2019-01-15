using MediatonicApi.Models;
using MediatonicApi.Models.Exceptions;
using MediatonicApi.Models.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading;

namespace Tests
{
    public class UserAnimalServiceTest
    {
        DbContextOptions<ApiContext> dbOptions;

        [SetUp]
        public void Setup()
        {
            dbOptions = new DbContextOptionsBuilder<ApiContext>()
                .UseInMemoryDatabase("user_animal_service_tests")
                .Options
            ;
        }

        [Test]
        public void TestAdd()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal"
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);

                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                service.Add(new UserAnimal() { UserId = user.Id, AnimalId = animal.Id });
            }

            // Make sure the user's animal was added to the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.UserAnimals.Count());
                Assert.AreEqual(user.Id, context.UserAnimals.First().UserId);
                Assert.AreEqual(animal.Id, context.UserAnimals.First().AnimalId);
            }
        }

        [Test]
        public void TestAddInvalidUser()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal"
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);

                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                Assert.Catch<NotFoundException>(() => service.Add(
                    new UserAnimal() { UserId = user.Id + 1, AnimalId = animal.Id }
                ));
            }

            // Make sure no data is in the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(0, context.UserAnimals.Count());
            }
        }

        [Test]
        public void TestAddInvalidAnimal()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal"
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);

                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                Assert.Catch<NotFoundException>(() => service.Add(
                    new UserAnimal() { UserId = user.Id, AnimalId = animal.Id + 1 }
                ));
            }

            // Make sure no data is in the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(0, context.UserAnimals.Count());
            }
        }

        [Test]
        public void TestAnimalAlreadyExists()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal"
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);
                context.UserAnimals.Add(new UserAnimal() { UserId = user.Id, AnimalId = animal.Id });
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                Assert.Catch<DuplicateEntryException>(() => service.Add(
                    new UserAnimal() { UserId = user.Id, AnimalId = animal.Id }
                ));
            }

            // Make sure the user's animal was added to the DB once only
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.UserAnimals.Count());
                Assert.AreEqual(user.Id, context.UserAnimals.First().UserId);
                Assert.AreEqual(animal.Id, context.UserAnimals.First().AnimalId);
            }
        }

        [Test]
        public void TestUpdate()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal",
                HungerPerSecond = 0.5m,
                SadnessPerSecond = 0.4m
            };

            UserAnimal userAnimal;
            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);

                userAnimal = new UserAnimal() {
                    UserId = user.Id,
                    AnimalId = animal.Id,
                };
                context.UserAnimals.Add(userAnimal);
                context.SaveChanges();
            }

            // Check the initial state of the animal
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.UserAnimals.Count());

                userAnimal = context.UserAnimals.Include(ua => ua.Animal).First();

                Assert.AreEqual(0, userAnimal.Hunger);
                Assert.AreEqual(0, userAnimal.Happiness);
            }

            // Add an update (at this stage it won't really update much)
            Thread.Sleep(1000);
            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                service.Update(userAnimal);
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.UserAnimals.Count());

                userAnimal = context.UserAnimals.Include(ua => ua.Animal).First();
                Assert.AreEqual(0.5, userAnimal.Hunger);
                Assert.AreEqual(-0.4, userAnimal.Happiness);
            }

            // Feed the animal and update again - this will cause the internal dates to update
            Thread.Sleep(1000);
            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                userAnimal.Feed(1);
                service.Update(userAnimal);
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.UserAnimals.Count());

                userAnimal = context.UserAnimals.Include(ua => ua.Animal).First();
                Assert.AreEqual(0, userAnimal.Hunger);
                Assert.AreEqual(-0.8, userAnimal.Happiness);
            }

            // Stroke the animal and update again - this will cause the internal dates to update
            Thread.Sleep(1000);
            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                userAnimal.Stroke(1);
                service.Update(userAnimal);
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.UserAnimals.Count());

                userAnimal = context.UserAnimals.Include(ua => ua.Animal).First();
                Assert.AreEqual(0.5, userAnimal.Hunger);
                Assert.AreEqual(0, userAnimal.Happiness);
            }
        }

        [Test]
        public void TestUpdateInvalid()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal",
                HungerPerSecond = 0.5m,
                SadnessPerSecond = 0.4m
            };

            UserAnimal userAnimal;
            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);

                userAnimal = new UserAnimal() {
                    UserId = user.Id,
                    AnimalId = animal.Id,
                };
                context.UserAnimals.Add(userAnimal);
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);
                userAnimal = context.UserAnimals.Include(ua => ua.Animal).First();

                // Test with an invalid user ID
                userAnimal.UserId = user.Id + 1;
                Assert.Catch<NotFoundException>(() => service.Update(userAnimal));

                // Test with an invalid animal ID
                userAnimal.UserId = user.Id;
                userAnimal.AnimalId = animal.Id + 1;
                Assert.Catch<NotFoundException>(() => service.Update(userAnimal));
            }
        }

        [Test]
        public void TestGet()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal",
                HungerPerSecond = 0.5m,
                SadnessPerSecond = 0.4m
            };

            User user2 = new User() {
                DisplayName = "Some display name 2"
            };

            Animal animal2 = new Animal() {
                TypeName = "Test animal 2",
                HungerPerSecond = 0.5m,
                SadnessPerSecond = 0.4m
            };

            UserAnimal userAnimal;
            UserAnimal userAnimal2;
            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);

                userAnimal = new UserAnimal() {
                    UserId = user.Id,
                    AnimalId = animal.Id,
                };

                userAnimal2 = new UserAnimal() {
                    UserId = user2.Id,
                    AnimalId = animal2.Id,
                };

                context.UserAnimals.Add(userAnimal);
                context.UserAnimals.Add(userAnimal2);
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                Assert.AreEqual(user.Id, service.Get(user.Id, animal.Id).UserId);
                Assert.AreEqual(animal.Id, service.Get(user.Id, animal.Id).AnimalId);
                Assert.IsNotNull(service.Get(user.Id, animal.Id).Animal);
            }
        }

        [Test]
        public void TestGetInvalid()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal",
                HungerPerSecond = 0.5m,
                SadnessPerSecond = 0.4m
            };

            User user2 = new User() {
                DisplayName = "Some display name 2"
            };

            Animal animal2 = new Animal() {
                TypeName = "Test animal 2",
                HungerPerSecond = 0.5m,
                SadnessPerSecond = 0.4m
            };

            UserAnimal userAnimal;
            UserAnimal userAnimal2;
            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);

                userAnimal = new UserAnimal() {
                    UserId = user.Id,
                    AnimalId = animal.Id,
                };

                userAnimal2 = new UserAnimal() {
                    UserId = user2.Id,
                    AnimalId = animal2.Id,
                };

                context.UserAnimals.Add(userAnimal);
                context.UserAnimals.Add(userAnimal2);
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                Assert.IsNull(service.Get(user.Id + 2, animal.Id));
                Assert.IsNull(service.Get(user.Id, animal.Id + 2));
            }
        }

        [Test]
        public void TestGetAll()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            Animal animal = new Animal() {
                TypeName = "Test animal",
                HungerPerSecond = 0.5m,
                SadnessPerSecond = 0.4m
            };

            User user2 = new User() {
                DisplayName = "Some display name 2"
            };

            Animal animal2 = new Animal() {
                TypeName = "Test animal 2",
                HungerPerSecond = 0.5m,
                SadnessPerSecond = 0.4m
            };

            UserAnimal userAnimal;
            UserAnimal userAnimal2;
            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Animals.Add(animal);

                userAnimal = new UserAnimal() {
                    UserId = user.Id,
                    AnimalId = animal.Id,
                };

                userAnimal2 = new UserAnimal() {
                    UserId = user2.Id,
                    AnimalId = animal2.Id,
                };

                context.UserAnimals.Add(userAnimal);
                context.UserAnimals.Add(userAnimal2);
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserAnimalService service = new UserAnimalService(context);

                Assert.AreEqual(1, service.GetAll(user.Id).Count());
                Assert.AreEqual(animal.Id, service.GetAll(user.Id).First().AnimalId);
                Assert.IsNotNull(service.GetAll(user.Id).First().Animal);
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
