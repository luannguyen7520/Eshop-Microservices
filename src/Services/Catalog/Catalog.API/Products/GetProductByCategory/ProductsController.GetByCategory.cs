using Catalog.API.Models;
using Catalog.API.Products.GetProductByCategory;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products;

public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public partial class ProductsController
{
    [HttpGet("{category}")]
    [ProducesResponseType<List<GetProductByCategoryResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByCategory([FromRoute] string category, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProductByCategoryQuery(category), cancellationToken);

        return Ok(result.Adapt<GetProductByCategoryResponse>());
    }
}
