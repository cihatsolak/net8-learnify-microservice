namespace Learnify.Discount.API.Features.Discounts.GetDiscountByCode;

public sealed record GetDiscountByCodeQuery(string Code) : IRequestResult<GetDiscountByCodeQueryResponse>;

public sealed record GetDiscountByCodeQueryResponse(string Code, float Rate);