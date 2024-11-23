using AutoMapper;
using FactoryAPI.Entities;
using FactoryAPI.Exceptions;
using FactoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryAPI.Services
{
    public interface IWorkerService
    {
        int Create(int factoryId, CreateWorkerDto dto);
        WorkerDto GetById(int factoryId, int workerId);
        List<WorkerDto> GetAll(int factoryId);
        void RemoveAll(int factoryId);
        void Update(int factoryId, int workerid, UpdateWorkerDto dto);
    }

    public class WorkerService : IWorkerService
    {
        private readonly FactoryDbContext _dbContext;
        private readonly IMapper _mapper;

        public WorkerService(FactoryDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        public int Create(int factoryId, CreateWorkerDto dto)
        {
            var factory = GetFactoryById(factoryId);
            var workerEntity = _mapper.Map<Worker>(dto);

            workerEntity.FactoryId = factoryId;

            _dbContext.Workers.Add(workerEntity);
            _dbContext.SaveChanges();

            return workerEntity.Id;
        }

        public WorkerDto GetById(int factoryId, int workerId)
        {
            var factory = GetFactoryById(factoryId);

            var worker = _dbContext.Workers.FirstOrDefault(d => d.Id == workerId);
            if (worker is null || worker.FactoryId != factoryId)
            {
                throw new NotFoundException("Worker not found");
            }

            var workerDto = _mapper.Map<WorkerDto>(worker);
            return workerDto;
        }

        public List<WorkerDto> GetAll(int factoryId)
        {

            var factory = GetFactoryById(factoryId);
            var workerDtos = _mapper.Map<List<WorkerDto>>(factory.Workers);

            return workerDtos;
        }

        public void RemoveAll(int factoryId)
        {
            var factory = GetFactoryById(factoryId);

            _dbContext.RemoveRange(factory.Workers);
            _dbContext.SaveChanges();

        }

        private Factory GetFactoryById(int factoryId)
        {
            var factory = _dbContext
                .Factories
                .Include(r => r.Workers)
                .FirstOrDefault(r => r.Id == factoryId);

            if (factory is null)
                throw new NotFoundException("Factory not found");

            return factory;
        }

        public void Update(int factoryId, int workerId, UpdateWorkerDto workerDto)
        {
            var factory = GetFactoryById(factoryId);
            var worker = _dbContext.Workers.FirstOrDefault(d => d.Id == workerId);

            if (worker is null || worker.FactoryId != factoryId)
                throw new NotFoundException("Worker not found");

            worker.FirstName = workerDto.FirstName;
            worker.LastName = workerDto.LastName;

            _dbContext.SaveChanges();
        }
    }
}
