using AndBank.Business.Interfaces;
using AndBank.Business.Models;
using AndBank.Process.Application.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndBank.Business.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;

        public PositionService(IPositionRepository positionRepository)
        {
            _repository = positionRepository;
        }

        public Task<PositionViewModel> GetClientById(string id)
        {
            var client = _repository.GetClientAsync(id);

            var result =  _mapper.Map<PositionViewModel>(client);

            return Task.FromResult(result);
        }

        public async Task<IEnumerable<PositionViewModel>> GetClientSymary(string id)
        {
            var clients = _repository.GetClientAsync(id);

            var result = _mapper.Map<List<PositionViewModel>>(clients);

            return await Task.FromResult(result);
        }

        public async Task PositionProcess(List<PositionModel> positionModel)
        {
            if (positionModel.Count == 0) return;

            try
            {
                await _repository.InsertAsync(positionModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na tentativa de gravar os dados no banco Pgsql {ex.ToString()}");
            }
           
        }

        public Task<IEnumerable<PositionViewModel>> TopClient(string id)
        {
            throw new NotImplementedException();
        }
    }
}
