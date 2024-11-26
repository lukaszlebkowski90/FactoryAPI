using FactoryAPI.Entities;
using FactoryAPI.Models;
using FactoryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [Route("api/factory")]
    [ApiController]
    public class FactoryController(IFactoryService factoryService) : ControllerBase
    {
        private readonly IFactoryService _factoryService = factoryService;

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<Factory>> GetAll()
        {
            var factories = _factoryService.GetAll();

            return Ok(factories);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("{id}")]
        public ActionResult<Factory> Get([FromRoute] int id)
        {
            var restaurant = _factoryService.GetById(id);

            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Factory> Create([FromBody] CreateFactoryDto dto)
        {
            var factoryId = _factoryService.Create(dto);


            return Created($"/api/factory/{factoryId}", null);
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateFactoryDto dto, [FromRoute] int id)
        {
            _factoryService.Update(id, dto);

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _factoryService.Delete(id);

            return NoContent();
        }


    }
}
