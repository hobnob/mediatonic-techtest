using MediatonicApi.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace MediatonicApi.Models.Services
{
    public class UserService : IService<User>
    {
        /// <summary>
        /// The context to use for this service
        /// </summary>
        private ApiContext _context;

        /// <summary>
        /// Creates a new user service
        /// </summary>
        /// <param name="context">The context to use for this service</param>
        public UserService(ApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="user">The user to add</param>
        /// <exception cref="DuplicateEntryException">Thrown if a user with that type already exists</exception>
        /// <exception cref="System.ArgumentException">Thrown if one of the user properties is incorrect</exception>
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

        /// <summary>
        /// Updates a user in the database
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <exception cref="System.NotImplementedException">This method is not implemented</exception>
        public void Update(User user)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds a single user in teh database with that user ID
        /// </summary>
        /// <param name="id">The ID to search for</param>
        /// <returns>A single user record, or null if not found</returns>
        public User FindOne(uint id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Finds all users in the database
        /// </summary>
        /// <returns>An enumerable of all users in the database</returns>
        public IEnumerable<User> FindAll()
        {
            return _context.Users;
        }
    }
}
