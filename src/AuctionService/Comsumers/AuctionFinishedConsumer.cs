using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Comsumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly AuctionDbContext _auctionDbContext;
        private readonly ILogger<AuctionFinishedConsumer> _logger;

        public AuctionFinishedConsumer(AuctionDbContext auctionDbContext, ILogger<AuctionFinishedConsumer> logger)
        {
            _auctionDbContext = auctionDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> consumeContext)
        {
            _logger.LogInformation("--> Consumer Auctiomn Finished");
            var auction = await _auctionDbContext.Auctions.FindAsync(consumeContext.Message.AuctionId);

            if (consumeContext.Message.ItemSold)
            {
                auction.Winner = consumeContext.Message.Winner;
                auction.SoldAmount = consumeContext.Message.Amount;
            }

            auction.Status = auction.SoldAmount > auction.ReservePrice ? Status.Finished : Status.ReserveNotMet;

            await _auctionDbContext.SaveChangesAsync();
        }
    }
}
