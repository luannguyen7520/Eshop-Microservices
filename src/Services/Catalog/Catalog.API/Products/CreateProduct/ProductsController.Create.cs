using Catalog.API.Products.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products;

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
public record CreateProductResponse(Guid Id);


public partial class ProductsController
{
    [HttpPost]
    [ProducesResponseType<CreateProductResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateProductCommand>();

        var result = await sender.Send(command, cancellationToken);

        var response = result.Adapt<CreateProductResponse>();

        return Created($"/api/products/{response.Id}", response);
    }
}
