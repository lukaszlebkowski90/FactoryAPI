using FactoryAPI.Entities;
using FactoryAPI.Models;
using FactoryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [Route("api/factories")]
    [ApiController]
    public class FactoryController : ControllerBase
    {
        private readonly IFactoryService _factoryService;
        public FactoryController(IFactoryService factoryService)
        {
            _factoryService = factoryService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Factory>> GetAll()
        {
            var factories = _factoryService.GetAll();

            return Ok(factories);
        }

        [HttpGet("{id}")]
        public ActionResult<Factory> Get([FromRoute] int id)
        {
            var restaurant = _factoryService.GetById(id);

            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [HttpPost]
        public ActionResult<Factory> Create([FromBody] CreateFactoryDto dto)
        {
            var factoryId = _factoryService.Create(dto);


            return Created($"/api/factories/{factoryId}", null);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateFactoryDto dto, [FromRoute] int id)
        {
            _factoryService.Update(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _factoryService.Delete(id);

            return NoContent();
        }


    }
}
