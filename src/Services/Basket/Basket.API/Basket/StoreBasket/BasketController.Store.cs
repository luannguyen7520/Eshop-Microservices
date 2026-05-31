using Basket.API.Basket.StoreBasket;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);

public partial class BasketController
{
    [HttpPost]
    [ProducesResponseType<StoreBasketResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StoreBasket([FromBody] StoreBasketRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<StoreBasketCommand>();
        var result = await sender.Send(command, cancellationToken);
        var response = result.Adapt<StoreBasketResponse>();

        return Created($"/api/basket/{result.UserName}", response);
    }
}
