using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MediatonicApi.Models.Services
{
    public class UserService
    {
        private ApiContext _context;

        public UserService(ApiContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            // Trim the display name - don't want odd spaces at the end
            user.DisplayName = user.DisplayName?.Trim();

            if (string.IsNullOrEmpty(user.DisplayName)) {
                throw new System.ArgumentException("Display name cannot be empty");
            }

            if (_context.Users.Any(u => u.DisplayName.ToLower() == user.DisplayName.ToLower())) {
                throw new System.ArgumentException("User already exists");
            }

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void AddAnimalToUser(uint userId, uint animalId)
        {
            // Get all user information
            User user = _context
                .Users
                .Include(u => u.Animals)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null) {
                throw new System.ArgumentException("User ID does not exist");
            }

            Animal animal = _context.Animals.FirstOrDefault(a => a.Id == animalId);
            if (animal == null) {
                throw new System.ArgumentException("Animal ID does not exist");
            }

            if (user.Animals.Any(a => a.AnimalId == animalId)) {
                throw new System.ArgumentException("User already owns '" + animal.TypeName + "'");
            }
            
            _context.Add(new UserAnimal() {
                User = user,
                UserId = userId,
                Animal = animal,
                AnimalId = animalId,
            });

            _context.SaveChanges();
        }

        public User Get(uint id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
