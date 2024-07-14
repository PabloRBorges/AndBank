using AndBank.Business.Interfaces;
using AndBank.Process.Application.ViewModel;
using AndBank.Processs.Aplication;
using AutoMapper;

namespace AndBank.Business.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;

        public PositionService(IPositionRepository positionRepository, IMapper mapper)
        {
            _repository = positionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Busca as posicões por Cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PositionViewModel>> GetPositionsClientById(string id)
        {
            var client = await _repository.GetClientAsync(id);

            var listresult = _mapper.Map<IEnumerable<PositionViewModel>>(client);

            //agrupa por position e retorna pela data ordenada
            listresult.GroupBy(p => p.PositionId)
            .Select(g => g.OrderByDescending(p => p.Date).First())
            .ToList();

            return listresult;
        }

        /// <summary>
        /// Faz um resumo dos valores por cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SummaryViewModel>> GetClientSummary(string id)
        {
            var clients = await _repository.GetClientAsync(id);

            var listresult = _mapper.Map<List<PositionViewModel>>(clients);
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

        /// <summary>
        /// Processa as informações e grava no banco
        /// </summary>
        /// <param name="positionModel"></param>
        /// <returns></returns>
        public async Task PositionProcess(List<PositionModel> positionModel)
        {
            if (positionModel.Count == 0) return;

            await _repository.InsertAsync(positionModel);
        }

        /// <summary>
        /// Busca os clientes com as maiores posições
        /// </summary>
        /// <param name="topNumber"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PositionViewModel>> TopClients(int topNumber)
        {
            var client = await _repository.GetTopClientsAsync(topNumber);

            var listresult = _mapper.Map<IEnumerable<PositionViewModel>>(client);

            //agrupa por valor e pega os 10 primeiros
            var top10Positions = listresult
                .OrderByDescending(p => p.Value)
                .Take(topNumber)
                .ToList();

            return listresult;
        }
    }
}
