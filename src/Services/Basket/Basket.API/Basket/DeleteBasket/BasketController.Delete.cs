using Basket.API.Basket.DeleteBasket;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket;

public record DeleteBasketResponse(ShoppingCart Cart);

public partial class BasketController
{
    [HttpDelete("{userName}")]
    [ProducesResponseType<DeleteBasketResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBasket([FromRoute] string userName, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteBasketCommand(userName), cancellationToken);

        return Ok(result.Adapt<DeleteBasketResponse>());
    }
}
