using Catalog.API.Models;
using Catalog.API.Products.DeleteProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products;

public record DeleteProductResponse(bool IsSuccess);

public partial class ProductsController
{
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType<DeleteProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleleProduct([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteProductCommand(id), cancellationToken);

        return Ok(result.Adapt<DeleteProductResponse>());
    }
}
