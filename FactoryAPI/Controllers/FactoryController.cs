using FactoryAPI.Entities;
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


    }
}
