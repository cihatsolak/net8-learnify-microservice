namespace Learnify.Basket.API.Features.Baskets.ApplyDiscountCoupon;

public sealed record ApplyDiscountCouponCommand(string Coupon, float DiscountRate) : IRequestResult;

public sealed class ApplyDiscountCouponCommandValidator : AbstractValidator<ApplyDiscountCouponCommand>
{
    public ApplyDiscountCouponCommandValidator()
    {
        RuleFor(x => x.Coupon).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.DiscountRate).GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");
    }
}