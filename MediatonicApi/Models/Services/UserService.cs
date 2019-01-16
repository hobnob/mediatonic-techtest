using MediatonicApi.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicApi.Models.Services
{
    public class UserService : IService<User>
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
                throw new DuplicateEntryException("User already exists");
            }

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            throw new System.NotImplementedException();
        }

        public User FindOne(uint id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<User> FindAll()
        {
            return _context.Users.ToArray();
        }
    }
}
