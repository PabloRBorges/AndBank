using AndBank.Core.Data;
using AndBank.Processs.Aplication;

namespace AndBank.Business.Interfaces
{
    public interface IPositionRepository: IRepository<PositionModel>
    {
        Task<IEnumerable<PositionModel>> GetClientAsync(string clienteId);
        Task<IEnumerable<PositionModel>> GetSummaryAsync(string clientId);
        Task<IEnumerable<PositionModel>> GetTopClientsAsync(int topNumber);
        Task InsertAsync(IEnumerable<PositionModel> positionModel);
    }
}
