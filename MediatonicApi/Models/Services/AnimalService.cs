using System.Linq;

namespace MediatonicApi.Models.Services
{
    public class AnimalService
    {
        private ApiContext _context;

        public AnimalService(ApiContext context)
        {
            _context = context;
        }

        public void Add(Animal animal)
        {
            if (_context.Animals.Any(a => a.TypeName.ToLower() == animal.TypeName.ToLower())) {
                throw new System.ArgumentException("Animal of this type already exists");
            }

            _context.Animals.Add(animal);
            _context.SaveChanges();
        }

        public Animal Get(uint id)
        {
            return _context.Animals.FirstOrDefault(a => a.Id == id);
        }
    }
}
