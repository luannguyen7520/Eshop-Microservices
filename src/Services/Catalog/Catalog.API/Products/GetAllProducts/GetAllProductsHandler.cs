using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten.Pagination;

namespace Catalog.API.Products.GetAllProducts;


public record class GetAllProductsQuery(string? FilterOn, string? FilterBy, string? SortBy, bool IsAscending, int PageNumber, int PageSize)
    : IQuery<GetAllProductsResult>;

public record class GetAllProductsResult(IEnumerable<Product> Products);

public class GetAllProductsHandler(IDocumentSession session)
    : IQueryHandler<GetAllProductsQuery, GetAllProductsResult>
{
    public async Task<GetAllProductsResult> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session
            .Query<Product>()
            .ToPagedListAsync(query.PageNumber, query.PageSize, cancellationToken);

        if (!string.IsNullOrWhiteSpace(query.FilterOn)
            && !string.IsNullOrWhiteSpace(query.FilterBy))
        {
            if (query.FilterOn.Equals(nameof(Product.Name), StringComparison.OrdinalIgnoreCase))
            {
                products = (IPagedList<Product>)products.Where(p => p.Name.Contains(query.FilterBy));
            }
        }

        if (string.IsNullOrWhiteSpace(query.SortBy))
        {

        }

        return new GetAllProductsResult(products);
    }
}
