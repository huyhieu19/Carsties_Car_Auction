using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    private readonly ILogger<AuctionDeletedConsumer> _logger;

    public AuctionDeletedConsumer(ILogger<AuctionDeletedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        _logger.LogInformation("--> Consuming auction deleted" + context.Message.Id);

        var result = await DB.DeleteAsync<Item>(context.Message.Id);

        if (!result.IsAcknowledged)
        {
            throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
        }
    }
}
