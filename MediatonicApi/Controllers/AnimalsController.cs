
using MediatonicApi.Models;
using MediatonicApi.Models.Exceptions;
using MediatonicApi.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MediatonicApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private AnimalService service;

        public AnimalsController(AnimalService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Animal>> Get()
        {
            return new JsonResult(service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Animal> Get(int id)
        {
            if (id < 0) {
                return new NotFoundResult();
            }

            Animal animal = service.Get((uint) id);
            if (animal == null) {
                return new NotFoundResult();
            }

            return new JsonResult(animal);
        }

        [HttpPost]
        public ActionResult<Animal> Post([FromBody] Animal animal)
        {
            try {
                service.Add(animal);

                return Get((int) animal.Id);
            } catch (DuplicateEntryException e) {
                return new BadRequestObjectResult(e.Message);
            } catch (System.ArgumentException e) {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
