using AutoMapper;
using Contracts;

namespace BiddingService;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Bid, BidDto>();
        CreateMap<Bid, BidPlaced>();
    }
}
