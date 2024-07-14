using AndBank.Business.Models;
using AndBank.Process.Application.ViewModel;

namespace AndBank.Business.Interfaces
{
    public interface IPositionService
    {
        Task PositionProcess(List<PositionModel> positionModel);
        Task<PositionViewModel> GetClientById(string id);
        Task<IEnumerable<PositionViewModel>> GetClientSymary(string id);
        Task<IEnumerable<PositionViewModel>> TopClient(string id);
    }
}
