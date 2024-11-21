using AutoMapper;
using FactoryAPI.Entities;
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
    public class FactoryService : IFactoryService
    {
        private readonly FactoryDbContext _dbContext;
        private readonly IMapper _mapper;

        public FactoryService(FactoryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int Create(CreateFactoryDto dto)
        {
            var factory = _mapper.Map<Factory>(dto);

            _dbContext.Add(factory);
            _dbContext.SaveChanges();

            return factory.Id;
        }

        public void Delete(int id)
        {
            var factory = _dbContext.Factories.FirstOrDefault(f => f.Id == id);

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

            return result;
        }

        public void Update(int id, UpdateFactoryDto factoryDto)
        {
            var factory = _dbContext
                .Factories
                .FirstOrDefault(r => r.Id == id);

            factory.Name = factoryDto.Name;
            factory.Description = factoryDto.Description;
            factory.ContactEmail = factoryDto.ContactEmail;

            _dbContext.SaveChanges();
        }

    }
}
