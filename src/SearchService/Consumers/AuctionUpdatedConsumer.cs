using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;
    private readonly ILogger<AuctionUpdatedConsumer> _logger;

    public AuctionUpdatedConsumer(IMapper mapper, ILogger<AuctionUpdatedConsumer> logger)
    {
        this._mapper = mapper;
        this._logger = logger;

    }

    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        _logger.LogInformation("---> Consuming auction updated: {id}", context.Message.Id);

        var item = _mapper.Map<Item>(context.Message);

        var result = await DB.Update<Item>()
            .Match(a => a.ID == context.Message.Id)
            .ModifyOnly(x => new
            {
                x.Color,
                x.Make,
                x.Model,
                x.Year,
                x.Mileage
            }, item)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
    }
}
