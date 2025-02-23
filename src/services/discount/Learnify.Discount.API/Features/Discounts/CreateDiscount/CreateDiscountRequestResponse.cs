﻿namespace Learnify.Discount.API.Features.Discounts.CreateDiscount;

public sealed record CreateDiscountCommand(string Code, float Rate, Guid UserId, DateTime Expired)
    : IRequestResult;

public sealed class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("{PropertyName} is required.").Length(10).WithMessage("{propertyName} must be 10 characters long");
        RuleFor(x => x.Rate).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(x => x.Expired).NotEmpty().WithMessage("{PropertyName} is required.");
    }
}