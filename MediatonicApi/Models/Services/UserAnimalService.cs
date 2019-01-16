using MediatonicApi.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicApi.Models.Services
{
    public class UserAnimalService : IService<UserAnimal>
    {
        private ApiContext _context;

        public UserAnimalService(ApiContext context)
        {
            _context = context;
        }

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

        public void Update(UserAnimal userAnimal)
        {
            if (!_context.UserAnimals.Any(ua => ua.AnimalId == userAnimal.AnimalId && ua.UserId == userAnimal.UserId)) {
                throw new NotFoundException("User/Animal combination does not exist");
            }

            _context.Update(userAnimal);
            _context.SaveChanges();
        }

        public UserAnimal FindOne(uint userId)
        {
            return _context
                .UserAnimals
                .Include(ua => ua.Animal)
                .FirstOrDefault(ua => ua.UserId == userId)
            ;
        }

        public IEnumerable<UserAnimal> FindAll()
        {
            return _context.UserAnimals.Include(ua => ua.Animal);
        }
    }
}
