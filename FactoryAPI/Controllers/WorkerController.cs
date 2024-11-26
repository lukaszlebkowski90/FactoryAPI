using FactoryAPI.Models;
using FactoryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [Route("api/factory/{factoryId}/worker")]
    [ApiController]
    public class WorkerController(IWorkerService workerService) : ControllerBase
    {
        private readonly IWorkerService _workerService = workerService;

        [Authorize]
        [HttpDelete]
        public ActionResult Delete([FromRoute] int factoryId)
        {
            _workerService.RemoveAll(factoryId);

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Post([FromRoute] int factoryId, [FromBody] CreateWorkerDto dto)
        {
            var newWorkerId = _workerService.Create(factoryId, dto);

            return Created($"api/factory/{factoryId}/worker/{newWorkerId}", null);
        }

        [Authorize]
        [HttpPut("{workerid}")]
        public ActionResult Update([FromBody] UpdateWorkerDto dto, [FromRoute] int factoryId, [FromRoute] int workerid)
        {
            _workerService.Update(workerid, factoryId, dto);

            return Ok();
        }

        [Authorize]
        [HttpGet("{workerId}")]
        public ActionResult<WorkerDto> Get([FromRoute] int factoryId, [FromRoute] int workerId)
        {
            WorkerDto worker = _workerService.GetById(factoryId, workerId);
            return Ok(worker);
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<WorkerDto>> Get([FromRoute] int factoryId)
        {
            var result = _workerService.GetAll(factoryId);
            return Ok(result);
        }

    }
}
