using AndBank.Business.Interfaces;
using AndBank.Process.Application.ViewModel;
using AndBank.Processs.Aplication;
using ApiFuncional.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

public class PositionControllerTests
{
    private readonly IPositionService _positionService;
    private readonly IPositionRepository _positionRepository;
    private readonly PositionController _controller;

    public PositionControllerTests()
    {
        _positionRepository = Substitute.For<IPositionRepository>();
        _positionService = Substitute.For<IPositionService>();
        _controller = new PositionController(_positionService);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WhenPositionsExist()
    {
        // Arrange
        var clienteId = "cliente1";
        var positions = new List<PositionViewModel> { new PositionViewModel() };
        _positionService.GetPositionsClientById(clienteId).Returns(positions);

        // Act
        var result = await _controller.Get(clienteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<PositionViewModel>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNoPositionsExist()
    {
        // Arrange
        var clienteId = "cliente1";
        _positionService.GetPositionsClientById(clienteId).Returns((List<PositionViewModel>)null);

        // Act
        var result = await _controller.Get(clienteId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetSummary_ReturnsOkResult_WhenSummaryExists()
    {
        // Arrange
        var clienteId = "cliente1";
        var summary = new List<SummaryViewModel>()
        {
            new SummaryViewModel()
            {
                ProductId = "asdf",
                TotalValue = 123
            }
        };
        _positionService.GetClientSummary(clienteId).Returns(summary);

        // Act
        var result = await _controller.GetSummary(clienteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);
        Assert.IsType<List<SummaryViewModel>>(okResult.Value);
    }

    [Fact]
    public async Task GetSummary_ReturnsNotFound_WhenNoSummaryExists()
    {
        // Arrange
        var clienteId = "cliente1";
        _positionRepository.GetClientAsync(clienteId).Returns(Enumerable.Empty<PositionModel>());
        _positionService.GetClientSummary(clienteId).Returns(Enumerable.Empty<SummaryViewModel>());

        // Act
        var result = await _controller.GetSummary(clienteId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetTop10_ReturnsOkResult_WhenTopClientsExist()
    {
        // Arrange
        var topClients = new List<PositionViewModel> { new PositionViewModel() };
        _positionService.TopClients(10).Returns(topClients);

        // Act
        var result = await _controller.GetTop10();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<PositionViewModel>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetTop10_ReturnsNotFound_WhenNoTopClientsExist()
    {
        // Arrange
        _positionService.TopClients(10).Returns((List<PositionViewModel>)null);

        // Act
        var result = await _controller.GetTop10();

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}