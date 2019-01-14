using MediatonicApi.Models;
using MediatonicApi.Models.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    class UserServiceTests
    {
        DbContextOptions<ApiContext> dbOptions;

        [SetUp]
        public void Setup()
        {
             dbOptions = new DbContextOptionsBuilder<ApiContext>()
               .UseInMemoryDatabase("user_service_tests")
               .Options;
        }

        [Test]
        public void TestAddingUser()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);

                service.Add(user);
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.Users.Count());
                Assert.IsTrue(context.Users.Any(u => u.DisplayName == user.DisplayName));
            }
        }

        [Test]
        public void TestAddingUserNoName()
        {

        }

        [Test]
        public void TestAddingUserSameName()
        {

        }

        [Test]
        public void TestAddAnimal()
        {

        }

        [Test]
        public void TestAnimalInvalidUser()
        {

        }

        [Test]
        public void TestAnimalInvalidAnimal()
        {

        }

        [Test]
        public void TestAnimalAlreadyExists()
        {

        }

        [Test]
        public void TestGet()
        {

        }

        [Test]
        public void TestGetInvalid()
        {

        }
    }
}
