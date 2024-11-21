using AutoMapper;
using FactoryAPI.Entities;
using FactoryAPI.Models;

namespace FactoryAPI
{
    public class FactoryMappingProfile : Profile
    {
        public FactoryMappingProfile()
        {
            CreateMap<Factory, FactoryDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Worker, WorkerDto>();

            CreateMap<CreateFactoryDto, Factory>()
                .ForMember(r => r.Address,
                    c => c.MapFrom(dto => new Address()
                    { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));

            CreateMap<CreateWorkerDto, Worker>();


        }
    }
}
