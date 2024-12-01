namespace Learnify.Basket.API.Features.Baskets;

public class BasketMapping : Profile
{
    public BasketMapping()
    {
        CreateMap<BasketResponse, Basket>().ReverseMap();
        CreateMap<BasketItemResponse, BasketItem>().ReverseMap();
    }
}
