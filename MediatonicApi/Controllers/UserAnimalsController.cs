using System.Collections.Generic;
using MediatonicApi.Models;
using MediatonicApi.Models.Exceptions;
using MediatonicApi.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediatonicApi.Controllers
{
    [Route("v1/users/{userId}/animals")]
    [ApiController]
    public class UserAnimalsController : ControllerBase
    {
        private UserAnimalService service;
        private const decimal FEED_AMOUNT = 0.25m;
        private const decimal STROKE_AMOUNT = 0.25m;

        public UserAnimalsController(UserAnimalService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserAnimal>> Get(uint userId)
        {
            return new JsonResult(service.GetAll(userId));
        }

        [HttpGet("{id}")]
        public ActionResult<UserAnimal> Get(uint userId, uint id)
        {
            UserAnimal userAnimal = service.Get(userId, id);
            if (userAnimal == null) {
                return new NotFoundResult();
            }

            return new JsonResult(userAnimal);
        }

        [HttpPost]
        public ActionResult<UserAnimal> Post(uint userId, [FromBody] uint animalId)
        {
            try {
                service.Add(new UserAnimal() { UserId = userId, AnimalId = animalId });

                return Get(userId, animalId);
            } catch (DuplicateEntryException e) {
                return new BadRequestObjectResult(e.Message);
            } catch (NotFoundException e) {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpGet("{id}/feed")]
        public ActionResult<UserAnimal> Feed(uint userId,  uint id)
        {
            UserAnimal userAnimal = service.Get(userId, id);
            if (userAnimal == null) {
                return new NotFoundResult();
            }

            // Feed the animal and update the database
            userAnimal.Feed(FEED_AMOUNT);
            service.Update(userAnimal);

            return Get(userId, id);
        }

        [HttpGet("{id}/stroke")]
        public ActionResult<UserAnimal> Stroke(uint userId, uint id)
        {
            UserAnimal userAnimal = service.Get(userId, id);
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
