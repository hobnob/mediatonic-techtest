
using System.Collections.Generic;

namespace MediatonicApi.Models.Services
{
    /// <summary>
    /// Interface for interacting with the database for the entity type (T)
    /// </summary>
    /// <typeparam name="T">The entity type to interact with</typeparam>
    public interface IService<T>
    {
        /// <summary>
        /// Adds the entity to the database
        /// </summary>
        /// <param name="entity">The entity to add</param>
        void Add(T entity);

        /// <summary>
        /// Updates the database with new entity details
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Finds a single entity with the specified ID
        /// </summary>
        /// <param name="id">The ID to find</param>
        /// <returns>The entity with the specified ID, or null if not found</returns>
        T FindOne(uint id);

        /// <summary>
        /// Finds all entities in the database
        /// </summary>
        /// <returns>All entities in the database</returns>
        IEnumerable<T> FindAll();
    }
}
