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
        void Update(int id, Factory dto);
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
            var factoryDto = _mapper.Map<Factory>(dto);
            _dbContext.Add(factoryDto);

            _dbContext.SaveChanges();

            return factoryDto.Id;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
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

        public void Update(int id, Factory factory)
        {
            var factoryToUpdate = _dbContext
                .Factories
                .FirstOrDefault(r => r.Id == id);


            factoryToUpdate.Name = factory.Name;
            factoryToUpdate.Description = factory.Description;
            factoryToUpdate.ContactEmail = factory.ContactEmail;
            factoryToUpdate.ContactNumber = factory.ContactNumber;

            _dbContext.SaveChanges();
        }

    }
}
