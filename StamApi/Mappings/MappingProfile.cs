using AutoMapper;
using Core.Domain.Models;
using StamApi.Models.Domain;

namespace StamApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DiagnoseCode, DiagnoseCodeDto>();
            CreateMap<TreatmentCode, TreatmentCodeDto>();
        }
    }
}