using Catalog.API.Models;
using Catalog.API.Products.GetAllProducts;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products;

public record GetAllProductsRequest(string? FilterOn, string? FilterBy, string? SortBy, bool IsAscending = true, int PageNumber = 1, int PageSize = 20);
public record GetAllProductsResponse(IEnumerable<Product> Products);

public partial class ProductsController
{
    [HttpGet]
    [ProducesResponseType<List<GetAllProductsResponse>>(StatusCodes.Status200OK)]
    public async Task<GetAllProductsResponse> GetAll(CancellationToken cancellationToken, [FromQuery] GetAllProductsRequest request)
    {
        var query = request.Adapt<GetAllProductsQuery>();

        var products = await sender.Send(query, cancellationToken);

        return products.Adapt<GetAllProductsResponse>();
    }
}
