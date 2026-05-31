namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName)
    : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(ShoppingCart Cart);


public class DeleteBasketHandler
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        return new DeleteBasketResult(new ShoppingCart(request.UserName));
    }
}
