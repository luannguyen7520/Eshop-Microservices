using Basket.API.Basket.GetBasket;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket;

public record GetBasketResponse(ShoppingCart Cart);

public partial class BasketController
{
    [HttpGet("{userName}")]
    [ProducesResponseType<GetBasketResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBasket([FromRoute] string userName, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetBasketQuery(userName), cancellationToken);

        return Ok(result.Adapt<GetBasketResponse>());
    }
}
