using AndBank.Business.Interfaces;
using AndBank.Core.Data;
using AndBank.Data.Context;
using AndBank.Processs.Aplication;
using Microsoft.EntityFrameworkCore;

namespace AndBank.Data.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly PositionDbContext _dbContext;
        const int batchSize = 5000;

        public PositionRepository(PositionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public async Task<IEnumerable<PositionModel>> GetClientAsync(string clienteId)
        {
           return await _dbContext.Positions.Where(x => x.ClientId == clienteId).ToListAsync(); 
        }

        public async Task InsertAsync(IEnumerable<PositionModel> positionModels)
        {
            int total = positionModels.Count();

            if (total < batchSize)
            {
              await  _dbContext.Positions.AddRangeAsync(positionModels);
            }
            else
            {
                for (int i = 0; i < total; i += batchSize)
                {
                    var batch = positionModels.Skip(i).Take(batchSize);
                    await _dbContext.Positions.AddRangeAsync(batch);
                    await _dbContext.SaveChangesAsync();
                    _dbContext.ChangeTracker.Clear(); 

                    Console.Write($"\rProgresso: [{new string('#', (i / batchSize))}{new string(' ', ((total - i) / batchSize))}] {i * 100 / total}%");
                }
                Console.Write($"\rProgresso: [{new string('#', (total / batchSize))}{new string(' ', ( 0 / batchSize))}] {total * 100 / total}%");
            }
        }

        public async Task<IEnumerable<PositionModel>> GetListClientAsync(string clientId)
        {
            return await _dbContext.Positions.ToListAsync();
        }

        public async Task<IEnumerable<PositionModel>> GetSummaryAsync(string clientId)
        {
            return await _dbContext.Positions.ToListAsync();
        }

        public async Task<IEnumerable<PositionModel>> GetAllAsync()
        {
            return await _dbContext.Positions.ToListAsync();
        }

        public async Task InsertAsync(PositionModel positionModel)
        {
            await _dbContext.AddAsync(positionModel);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
