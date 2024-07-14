using AndBank.Business.Models;
using AndBank.Core.Data;

namespace AndBank.Business.Interfaces
{
    public interface IPositionRepository: IRepository<PositionModel>
    {
        Task<PositionModel> GetClientAsync(string clientId);
        Task<IEnumerable<PositionModel>> GetListClientAsync(string clientId);
        Task<IEnumerable<PositionModel>> GetSummaryAsync(string clientId);
        Task<IEnumerable<PositionModel>> GetAllAsync();
        Task InsertAsync(IEnumerable<PositionModel> positionModel);
        Task InsertAsync(PositionModel positionModel);
    }
}
