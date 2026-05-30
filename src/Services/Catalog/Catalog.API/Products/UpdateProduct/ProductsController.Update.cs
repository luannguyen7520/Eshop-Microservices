using Catalog.API.Products.UpdateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products;

public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
public record UpdateProductResponse(bool IsSuccess);

public partial class ProductsController
{
    [HttpPut("{id:Guid}")]
    [ProducesResponseType<UpdateProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateProductCommand>();

        var result = await sender.Send(command, cancellationToken);

        return Ok(result.Adapt<UpdateProductResponse>());
    }
}
