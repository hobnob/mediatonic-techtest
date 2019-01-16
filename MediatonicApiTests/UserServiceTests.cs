using MediatonicApi.Models;
using MediatonicApi.Models.Exceptions;
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
                .Options
            ;
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

            // Make sure the user was added to the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.Users.Count());
                Assert.AreNotEqual(0, context.Users.First().Id);
                Assert.IsTrue(context.Users.Any(u => u.DisplayName == user.DisplayName));
            }

            // Make sure that white space is trimmed
            string newUserName = "  New display name  ";
            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);

                User user2 = new User() {
                    DisplayName = newUserName
                };
                
                service.Add(user2);
            }

            // Make sure the user was added to the DB and auto-increment works
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(2, context.Users.Count());
                Assert.AreNotEqual(0, context.Users.Skip(1).First().Id);
                Assert.IsTrue(context.Users.Any(u => u.DisplayName == newUserName.Trim()));
            }
        }

        [Test]
        public void TestAddingUserNoName()
        {
            User user = new User() {
                DisplayName = ""
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);

                // Test with name as an empty string
                Assert.Catch<System.ArgumentException>(() => service.Add(user));

                // Test with a name of just white space
                user.DisplayName = "     ";
                Assert.Catch<System.ArgumentException>(() => service.Add(user));

                user.DisplayName = null;
                Assert.Catch<System.ArgumentException>(() => service.Add(user));
            }

            // Make sure no users are in the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(0, context.Users.Count());
            }
        }

        [Test]
        public void TestAddingUserSameName()
        {
            string originalDisplayName = "Some display name";
            User user = new User() {
                DisplayName = originalDisplayName
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);

                service.Add(user);
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);

                Assert.Catch<DuplicateEntryException>(() => service.Add(user));

                // Test with different casing
                user.DisplayName = "some Display Name";
                Assert.Catch<DuplicateEntryException>(() => service.Add(user));

                user.DisplayName = "    " + user.DisplayName + "   ";
                Assert.Catch<DuplicateEntryException>(() => service.Add(user));
            }

            // Make sure the user was added to the DB
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(1, context.Users.Count());
                Assert.IsTrue(context.Users.Any(u => u.DisplayName == originalDisplayName));
            }
        }

        [Test]
        public void TestAddUserExistingId()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            User user2 = new User() {
                DisplayName = "Some other display name"
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.SaveChanges();
            }

            // Set the second user to have a duplicate identifier
            user2.Id = user.Id;

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);
                service.Add(user2);
            }

            // Make sure 2 animals are in the DB and are correct
            using (ApiContext context = new ApiContext(dbOptions)) {
                Assert.AreEqual(2, context.Users.Count());
                Assert.AreEqual(user.Id, context.Users.First().Id);
                Assert.AreEqual(user.DisplayName, context.Users.First().DisplayName);
                Assert.AreEqual(user2.Id, context.Users.Last().Id);
                Assert.AreEqual(user2.DisplayName, context.Users.Last().DisplayName);
            }
        }

        [Test]
        public void TestFindOne()
        {
            string displayName = "Some display name";
            User user = new User() {
                DisplayName = displayName
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Users.Add(new User() { DisplayName = "Another random user" });
                context.SaveChanges();
            }
            
            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);

                Assert.AreNotEqual(null, service.FindOne(user.Id));
                Assert.AreEqual(displayName, service.FindOne(user.Id).DisplayName);
            }
        }

        [Test]
        public void TestFindOneInvalid()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Users.Add(new User() { DisplayName = "Another random user" });
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);

                Assert.IsNull(service.FindOne(user.Id + 2));
            }
        }

        [Test]
        public void TestFindAll()
        {
            User user = new User() {
                DisplayName = "Some display name"
            };

            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Users.Add(user);
                context.Users.Add(new User() { DisplayName = "Another random user" });
                context.SaveChanges();
            }

            using (ApiContext context = new ApiContext(dbOptions)) {
                UserService service = new UserService(context);
                
                Assert.AreEqual(2, service.FindAll().Count());
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Remove everything in the DB - ready to test again
            using (ApiContext context = new ApiContext(dbOptions)) {
                context.Database.EnsureDeleted();
            }
        }
    }
}
