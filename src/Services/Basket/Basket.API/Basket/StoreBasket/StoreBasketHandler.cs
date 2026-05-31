namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Card)
    : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Card).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Card.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBasketCommandHandler(IBasketRepository repository)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await repository.StoreBasket(command.Card, cancellationToken);

        return new StoreBasketResult(result.UserName);
    }
}
