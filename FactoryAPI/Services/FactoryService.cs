using AutoMapper;
using FactoryAPI.Entities;
using FactoryAPI.Exceptions;
using FactoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryAPI.Services
{
    public interface IFactoryService
    {
        IEnumerable<FactoryDto> GetAll();
        FactoryDto GetById(int id);
        int Create(CreateFactoryDto dto);
        void Delete(int id);
        void Update(int id, UpdateFactoryDto dto);
    }
    public class FactoryService(FactoryDbContext dbContext, IMapper mapper, ILogger<FactoryService> logger, IUserContextService userContextService) : IFactoryService
    {
        private readonly FactoryDbContext _dbContext = dbContext;
        private readonly ILogger<FactoryService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IUserContextService _userContextService = userContextService;

        public int Create(CreateFactoryDto dto)
        {
            var factory = _mapper.Map<Factory>(dto);
            factory.CreatedById = _userContextService.GetUserId;
            _dbContext.Add(factory);
            _dbContext.SaveChanges();

            _logger.LogInformation($"Factory with id: {factory.Id} was created");

            return factory.Id;
        }

        public void Delete(int id)
        {
            _logger.LogError($"Factory with id: {id} DELETE action invoked");
            var factory = _dbContext.Factories.FirstOrDefault(f => f.Id == id);

            if (factory is null)
                throw new NotFoundException("Restaurant not found");

            _dbContext.Remove(factory);
            _dbContext.SaveChanges();
        }

        public IEnumerable<FactoryDto> GetAll()
        {
            var factory = _dbContext
                .Factories
                .Include(r => r.Address)
                .Include(r => r.Workers)
                .ToList();

            var result = _mapper.Map<List<FactoryDto>>(factory);

            return result;
        }

        public FactoryDto GetById(int id)
        {
            var factory = _dbContext
                .Factories
                .Include(r => r.Address)
                .Include(r => r.Workers)
                .FirstOrDefault(f => f.Id == id);

            var result = _mapper.Map<FactoryDto>(factory);

            if (factory is null)
                throw new NotFoundException("Factory not found");

            return result;
        }

        public void Update(int id, UpdateFactoryDto factoryDto)
        {
            var factory = _dbContext
                .Factories
                .FirstOrDefault(r => r.Id == id);

            if (factory is null)
                throw new NotFoundException("Restaurant not found");
            factory.Name = factoryDto.Name;
            factory.Description = factoryDto.Description;
            factory.ContactEmail = factoryDto.ContactEmail;

            _dbContext.SaveChanges();
        }

    }
}
