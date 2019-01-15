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
        private UserService service;

        public UsersController(UserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return new JsonResult(service.GetAll());
        }
        
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            if (id < 0) {
                return new NotFoundResult();
            }

            User user = service.Get((uint) id);
            if (user == null) {
                return new NotFoundResult();
            }

            return new JsonResult(user);
        }
        
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            try {
                service.Add(user);

                return Get((int) user.Id);
            } catch (DuplicateEntryException e) {
                return new BadRequestObjectResult(e.Message);
            } catch (System.ArgumentException e) {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
