using AndBank.Process.Application.ViewModel;
using AndBank.Processs.Aplication;

namespace AndBank.Business.Interfaces
{
    public interface IPositionService
    {
        Task PositionProcess(List<PositionModel> positionModel);
        Task<IEnumerable<PositionViewModel>> GetPositionsClientById(string id);
        Task<IEnumerable<SummaryViewModel>> GetClientSummary(string id);
        Task<IEnumerable<PositionViewModel>> TopClient(string id);
    }
}
