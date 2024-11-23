using FactoryAPI.Models;
using FactoryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [Route("api/factory/{factoryId}/worker")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int factoryId)
        {
            _workerService.RemoveAll(factoryId);

            return NoContent();
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int factoryId, [FromBody] CreateWorkerDto dto)
        {
            var newWorkerId = _workerService.Create(factoryId, dto);

            return Created($"api/factory/{factoryId}/worker/{newWorkerId}", null);
        }

        [HttpPut("{workerid}")]
        public ActionResult Update([FromBody] UpdateWorkerDto dto, [FromRoute] int factoryId, [FromRoute] int workerid)
        {
            _workerService.Update(workerid, factoryId, dto);

            return Ok();
        }

        [HttpGet("{workerId}")]
        public ActionResult<WorkerDto> Get([FromRoute] int factoryId, [FromRoute] int workerId)
        {
            WorkerDto worker = _workerService.GetById(factoryId, workerId);
            return Ok(worker);
        }

        [HttpGet]
        public ActionResult<List<WorkerDto>> Get([FromRoute] int factoryId)
        {
            var result = _workerService.GetAll(factoryId);
            return Ok(result);
        }

    }
}
