using AndBank.Process.Application.ViewModel;
using AndBank.Processs.Aplication;
using AutoMapper;

namespace AndBank.Process.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<PositionViewModel, PositionModel>();
            CreateMap<List<PositionViewModel>, List<PositionViewModel>>();
        }
    }
}
