using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Comsumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _auctionDbContext;
        private readonly ILogger<BidPlacedConsumer> _logger;

        public BidPlacedConsumer(AuctionDbContext auctionDbContext, ILogger<BidPlacedConsumer> logger)
        {
            _auctionDbContext = auctionDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BidPlaced> consumeContext)
        {
            _logger.LogInformation("--> Consumer BidPlacedConsumer");
            var auction = await DB.Fin.Auctions.FindAsync(consumeContext.Message.AuctionId);

            if (consumeContext.Message.BidStatus.Contains("Accepted")
           && consumeContext.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = consumeContext.Message.Amount;
                await _auctionDbContext.SaveChangesAsync();
            }
        }
    }
}
