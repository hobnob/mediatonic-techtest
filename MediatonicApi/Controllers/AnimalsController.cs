using MediatonicApi.Models;
using MediatonicApi.Models.Exceptions;
using MediatonicApi.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MediatonicApi.Controllers
{
    /// <summary>
    /// Controller for animal routes
    /// </summary>
    [Route("v1/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        /// <summary>
        /// Holds the animals service provider
        /// </summary>
        private AnimalService service;

        /// <summary>
        /// Creates a new animals controller
        /// </summary>
        /// <param name="service">Injected animals service</param>
        public AnimalsController(AnimalService service)
        {
            this.service = service;
        }
        
        /// <summary>
        /// Gets all animals
        /// </summary>
        /// <returns>An array of all possible animals</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<Animal>> Get()
        {
            return new JsonResult(service.GetAll());
        }

        /// <summary>
        /// Gets a single animal by ID
        /// </summary>
        /// <param name="id">The ID to look for</param>
        /// <returns>A single animal</returns>
        /// <response code="404">Animal with that ID could not be found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Animal> Get(uint id)
        {
            Animal animal = service.Get(id);
            if (animal == null) {
                return new NotFoundResult();
            }

            return new JsonResult(animal);
        }

        /// <summary>
        /// Creates a new animal with the specified properties
        /// </summary>
        /// <param name="animal">The animal to create</param>
        /// <returns>The newley created animal</returns>
        /// <response code="400">An invalid parameter has been provided. Check the returned string for details</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<Animal> Post([FromBody] Animal animal)
        {
            try {
                service.Add(animal);

                return new CreatedAtActionResult("Get", "Animals", new { id = animal.Id }, animal);
            } catch (DuplicateEntryException e) {
                return new BadRequestObjectResult(e.Message);
            } catch (System.ArgumentException e) {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
