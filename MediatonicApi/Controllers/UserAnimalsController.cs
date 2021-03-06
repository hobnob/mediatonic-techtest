﻿using System.Collections.Generic;
using System.Linq;
using MediatonicApi.Models;
using MediatonicApi.Models.Exceptions;
using MediatonicApi.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediatonicApi.Controllers
{
    [Route("v1/Users/{userId}/Animals")]
    [ApiController]
    public class UserAnimalsController : ControllerBase
    {
        /// <summary>
        /// Holds the user animals service provider
        /// </summary>
        private IService<UserAnimal> service;


        /// <summary>
        /// Holds the user service provider
        /// </summary>
        private IService<User> userService;

        /// <summary>
        /// The amount that feeding an animal adds
        /// </summary>
        private const decimal FEED_AMOUNT = 0.25m;

        /// <summary>
        /// The amount taht stroking an animal adds
        /// </summary>
        private const decimal STROKE_AMOUNT = 0.25m;

        /// <summary>
        /// Creates a new user animals controller
        /// </summary>
        /// <param name="service">Injected user animals service</param>
        /// <param name="userService">Injected user service</param>
        public UserAnimalsController(IService<UserAnimal> service, IService<User> userService)
        {
            this.service = service;
            this.userService = userService;
        }

        /// <summary>
        /// Gets all animals assigned to a specific user
        /// </summary>
        /// <param name="userId">The user to get animals for</param>
        /// <returns>An array of animals that the user owns</returns>
        /// <response code="404">User with that ID could not be found</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserAnimal>> Get(uint userId)
        {
            if (userService.FindOne(userId) == null) {
                return new NotFoundResult();
            }

            return new JsonResult(service.FindAll().Where(ua => ua.UserId == userId));
        }

        /// <summary>
        /// Gets a single animal that the user owns
        /// </summary>
        /// <param name="userId">The user to get animal for</param>
        /// <param name="id">The animal ID to get</param>
        /// <returns>The details of the animal that the user owns</returns>
        /// <response code="404">Animal or User with that ID could not be found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserAnimal> Get(uint userId, uint id)
        {
            UserAnimal userAnimal = service
                .FindAll()
                .FirstOrDefault(ua => ua.UserId == userId && ua.AnimalId == id)
            ;

            if (userAnimal == null) {
                return new NotFoundResult();
            }

            return new JsonResult(userAnimal);
        }

        /// <summary>
        /// Sets a user to own a new type of animal
        /// </summary>
        /// <param name="userId">The user to assign ownership to</param>
        /// <param name="animalId">The animal ID to assign to the user</param>
        /// <returns>The location of the new User Animal entity</returns>
        /// <response code="400">Animal owned by user with that ID does not exist</response>
        /// <response code="404">User with that ID could not be found</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<UserAnimal> Post(uint userId, [FromBody] uint animalId)
        {
            if (userService.FindOne(userId) == null) {
                return new NotFoundResult();
            }

            try {
                UserAnimal userAnimal = new UserAnimal() { UserId = userId, AnimalId = animalId };
                service.Add(userAnimal);

                return new CreatedAtActionResult("Get", "UserAnimals", new { userId, id = animalId }, userAnimal);
            } catch (DuplicateEntryException e) {
                return new BadRequestObjectResult(e.Message);
            } catch (NotFoundException e) {
                return new BadRequestObjectResult(e.Message);
            }
        }

        /// <summary>
        /// Feeds the animal specified by the user ID and animal ID
        /// </summary>
        /// <param name="userId">The ID of the user that owns the animal</param>
        /// <param name="id">The ID of the animal to feed</param>
        /// <returns>The new status for that particular animal</returns>
        /// <response code="404">Animal or User with that ID could not be found</response>
        [HttpPut("{id}/Feed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserAnimal> Feed(uint userId,  uint id)
        {
            UserAnimal userAnimal = service
                .FindAll()
                .FirstOrDefault(ua => ua.UserId == userId && ua.AnimalId == id)
            ;

            if (userAnimal == null) {
                return new NotFoundResult();
            }

            // Feed the animal and update the database
            userAnimal.Feed(FEED_AMOUNT);
            service.Update(userAnimal);

            return Get(userId, id);
        }

        /// <summary>
        /// Strokes the animal specified by the user ID and animal ID
        /// </summary>
        /// <param name="userId">The ID of the user that owns the animal</param>
        /// <param name="id">The ID of the animal to stroke</param>
        /// <returns>The new status for that particular animal</returns>
        /// <response code="404">Animal or User with that ID could not be found</response>
        [HttpPut("{id}/Stroke")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<UserAnimal> Stroke(uint userId, uint id)
        {
            UserAnimal userAnimal = service
                .FindAll()
                .FirstOrDefault(ua => ua.UserId == userId && ua.AnimalId == id)
            ;

            if (userAnimal == null) {
                return new NotFoundResult();
            }

            // Stroke the animal and update the database
            userAnimal.Stroke(STROKE_AMOUNT);
            service.Update(userAnimal);

            return Get(userId, id);
        }
    }
}
