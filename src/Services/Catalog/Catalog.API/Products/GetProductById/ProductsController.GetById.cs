using Catalog.API.Models;
using Catalog.API.Products.GetProductById;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products;

public record GetProductByIdResponse(Product Product);

public partial class ProductsController
{
    [HttpGet("{id:Guid}")]
    [ProducesResponseType<GetProductByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProductByIdQuery(id), cancellationToken);

        return Ok(result.Adapt<GetProductByIdResponse>());
    }
}
