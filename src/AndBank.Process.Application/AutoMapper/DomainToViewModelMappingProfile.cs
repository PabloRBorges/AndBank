using AndBank.Process.Application.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndBank.Process.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
                CreateMap<PositionViewModel, PositionViewModel>();
                CreateMap<List<PositionViewModel>, List<PositionViewModel>>();  
        }
    }
}
