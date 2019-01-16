using MediatonicApi.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicApi.Models.Services
{
    public class UserAnimalService : IService<UserAnimal>
    {
        /// <summary>
        /// The context to use for this service
        /// </summary>
        private ApiContext _context;

        /// <summary>
        /// Creates a new user animal service
        /// </summary>
        /// <param name="context">The context to use for this service</param>
        public UserAnimalService(ApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new animal to a user through the database
        /// </summary>
        /// <param name="userAnimal">The user's animal to add</param>
        /// <exception cref="NotFoundException">Thrown if the user or animal aren't found</exception>
        /// <exception cref="DuplicateEntryException">Thrown if the usr already owns the animal</exception>
        public void Add(UserAnimal userAnimal)
        {
            // Get all user information
            User user = _context
                .Users
                .Include(u => u.Animals)
                .FirstOrDefault(u => u.Id == userAnimal.UserId);

            if (user == null) {
                throw new NotFoundException("User ID does not exist");
            }

            Animal animal = _context.Animals.FirstOrDefault(a => a.Id == userAnimal.AnimalId);
            if (animal == null) {
                throw new NotFoundException("Animal ID does not exist");
            }

            if (user.Animals.Any(a => a.AnimalId == userAnimal.AnimalId)) {
                throw new DuplicateEntryException("User already owns '" + animal.TypeName + "'");
            }

            _context.Add(userAnimal);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates the user's animal details in the database
        /// </summary>
        /// <param name="userAnimal">The user animal to update</param>
        /// <exception cref="NotFoundException">Thrown if the user/animal combo doesn't exist</exception>
        public void Update(UserAnimal userAnimal)
        {
            if (!_context.UserAnimals.Any(ua => ua.AnimalId == userAnimal.AnimalId && ua.UserId == userAnimal.UserId)) {
                throw new NotFoundException("User/Animal combination does not exist");
            }

            _context.Update(userAnimal);
            _context.SaveChanges();
        }

        /// <summary>
        /// Returns the first animal a user owns for the specified user ID
        /// </summary>
        /// <param name="userId">The ID to search for</param>
        /// <returns>The first animal for the user specified or null if not found</returns>
        public UserAnimal FindOne(uint userId)
        {
            return _context
                .UserAnimals
                .Include(ua => ua.Animal)
                .FirstOrDefault(ua => ua.UserId == userId)
            ;
        }

        /// <summary>
        /// Finds all user animals from the database
        /// </summary>
        /// <returns>An enumerable of user animals in the database</returns>
        public IEnumerable<UserAnimal> FindAll()
        {
            return _context.UserAnimals.Include(ua => ua.Animal);
        }
    }
}
