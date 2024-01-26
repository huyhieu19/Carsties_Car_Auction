using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;
    private readonly ILogger<AuctionCreatedConsumer> _logger;

    public AuctionCreatedConsumer(IMapper mapper, ILogger<AuctionCreatedConsumer> logger)
    {
        this._mapper = mapper;
        this._logger = logger;

    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        _logger.LogInformation("---> Consuming auction created: {id}", context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        if (item.Model == "Foo") throw new ArgumentException("Cannot sell cars with name of Foo");

        await item.SaveAsync();
    }
}
