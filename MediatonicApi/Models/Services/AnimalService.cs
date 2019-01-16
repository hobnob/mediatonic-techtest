using MediatonicApi.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicApi.Models.Services
{
    public class AnimalService : IService<Animal>
    {
        private ApiContext _context;

        public AnimalService(ApiContext context)
        {
            _context = context;
        }

        public void Add(Animal animal)
        {
            // Trim the name - don't want odd spaces at the end
            animal.TypeName = animal.TypeName?.Trim();

            if (string.IsNullOrEmpty(animal.TypeName)) {
                throw new System.ArgumentException("Animal type cannot be empty");
            }

            if (_context.Animals.Any(a => a.TypeName.ToLower() == animal.TypeName.ToLower())) {
                throw new DuplicateEntryException("Animal of this type already exists");
            }

            // Make sure that Hunger actually moves
            if (animal.HungerPerSecond <= 0) {
                throw new System.ArgumentException("Hunger per second has to be bigger than zero");
            }

            // Make sure happiness actually moves
            if (animal.SadnessPerSecond <= 0) {
                throw new System.ArgumentException("Sadness per second has to be bigger than zero");
            }

            _context.Animals.Add(animal);
            _context.SaveChanges();
        }

        public void Update(Animal animal)
        {
            throw new System.NotImplementedException();
        }

        public Animal FindOne(uint id)
        {
            return _context.Animals.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Animal> FindAll()
        {
            return _context.Animals;
        }
    }
}
