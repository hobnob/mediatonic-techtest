using MediatonicApi.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicApi.Models.Services
{
    public class AnimalService : IService<Animal>
    {
        /// <summary>
        /// The context to use for this service
        /// </summary>
        private ApiContext _context;

        /// <summary>
        /// Creates a new animal service
        /// </summary>
        /// <param name="context">The context to use for this service</param>
        public AnimalService(ApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new animal to the database
        /// </summary>
        /// <param name="animal">The animal to add</param>
        /// <exception cref="DuplicateEntryException">Thrown if an animal with that type already exists</exception>
        /// <exception cref="System.ArgumentException">Thrown if one of the animal properties is incorrect</exception>
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

        /// <summary>
        /// Updates an animal in the database
        /// </summary>
        /// <param name="animal">The animal to update</param>
        /// <exception cref="System.NotImplementedException">This method is not implemented</exception>
        public void Update(Animal animal)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds a single animal with the specified ID
        /// </summary>
        /// <param name="id">The ID to search for</param>
        /// <returns>The animal with teh specified ID, or null if not found</returns>
        public Animal FindOne(uint id)
        {
            return _context.Animals.FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Finds all animals in the database
        /// </summary>
        /// <returns>An enurable of all the animals in te database</returns>
        public IEnumerable<Animal> FindAll()
        {
            return _context.Animals;
        }
    }
}
