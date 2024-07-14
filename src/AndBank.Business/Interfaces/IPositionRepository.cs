using AndBank.Core.Data;
using AndBank.Processs.Aplication;

namespace AndBank.Business.Interfaces
{
    public interface IPositionRepository: IRepository<PositionModel>
    {
        Task<IEnumerable<PositionModel>> GetClientAsync(string clienteId);
        Task<IEnumerable<PositionModel>> GetListClientAsync(string clientId);
        Task<IEnumerable<PositionModel>> GetSummaryAsync(string clientId);
        Task<IEnumerable<PositionModel>> GetAllAsync();
        Task InsertAsync(IEnumerable<PositionModel> positionModel);
        Task InsertAsync(PositionModel positionModel);
    }
}
