using AndBank.Business.Interfaces;
using AndBank.Process.Application.ViewModel;
using AndBank.Processs.Aplication;
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

        public async Task<IEnumerable<PositionViewModel>> GetPositionsClientById(string id)
        {
            var client = await _repository.GetClientAsync(id);

            //var result =  _mapper.Map<IEnumerable<PositionViewModel>>(client);
            var listresult = new List<PositionViewModel>();

            foreach (var position in client)
            {
                var result = new PositionViewModel()
                {
                    Value = position.Value,
                    ClientId = position.ClientId,
                    Date = position.Date,
                    PositionId = position.PositionId,
                    ProductId = position.ProductId,
                    Quantity = position.Quantity,
                };
                listresult.Add(result);
            }

            //agrupa por position e retorna pela data ordenada
            listresult.GroupBy(p => p.PositionId)
            .Select(g => g.OrderByDescending(p => p.Date).First())
            .ToList();

            return listresult;
        }

        public async Task<IEnumerable<SummaryViewModel>> GetClientSummary(string id)
        {
            var clients =await _repository.GetClientAsync(id);

           // var result = _mapper.Map<List<PositionViewModel>>(clients);
            var listresult = new List<PositionViewModel>();

            foreach (var position in clients)
            {
                var result = new PositionViewModel()
                {
                    Value = position.Value,
                    ClientId = position.ClientId,
                    Date = position.Date,
                    PositionId = position.PositionId,
                    ProductId = position.ProductId,
                    Quantity = position.Quantity,
                };
                listresult.Add(result);
            }

           var summaryList = listresult
           .GroupBy(p => p.ProductId)
           .Select(g => new SummaryViewModel
           {
               ProductId = g.Key,
               TotalValue = g.Sum(p => p.Value)
           })
           .ToList();


            return summaryList;
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
