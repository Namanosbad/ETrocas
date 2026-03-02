using AutoMapper;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Entities;

namespace ETrocas.Application.Mappings
{
    public class PropostaMappingProfile : Profile
    {
        public PropostaMappingProfile()
        {
            CreateMap<PropostaRequest, Proposta>()
                .ForMember(dest => dest.ProdutoOfertadoId, opt => opt.MapFrom(src => src.ProdutoOfertadoid));

            CreateMap<Proposta, PropostaResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusProposta));
        }
    }
}
