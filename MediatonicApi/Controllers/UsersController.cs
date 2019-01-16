using System.Collections.Generic;
using MediatonicApi.Models;
using MediatonicApi.Models.Exceptions;
using MediatonicApi.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediatonicApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Holds the user service provider
        /// </summary>
        private IService<User> service;

        /// <summary>
        /// Creates a new user controller
        /// </summary>
        /// <param name="service">Injected users service</param>
        public UsersController(IService<User> service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>An array of all possible users</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<User>> Get()
        {
            return new JsonResult(service.FindAll());
        }

        /// <summary>
        /// Gets a single user by ID
        /// </summary>
        /// <param name="id">The ID to look for</param>
        /// <returns>A single user</returns>
        /// <response code="404">User with that ID could not be found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<User> Get(uint id)
        {
            User user = service.FindOne(id);
            if (user == null) {
                return new NotFoundResult();
            }

            return new JsonResult(user);
        }

        /// <summary>
        /// Creates a new user with the specified properties
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <returns>The newley created user</returns>
        /// <response code="400">An invalid parameter has been provided. Check the returned string for details</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<User> Post([FromBody] User user)
        {
            try {
                service.Add(user);

                return Get(user.Id);
            } catch (DuplicateEntryException e) {
                return new BadRequestObjectResult(e.Message);
            } catch (System.ArgumentException e) {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
