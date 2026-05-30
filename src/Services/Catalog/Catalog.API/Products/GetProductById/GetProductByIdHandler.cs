using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;

namespace Catalog.API.Products.GetProductById;

public record class GetProductByIdQuery(Guid Id)
    : IQuery<GetProductByIdResult>;
public record class GetProductByIdResult(Product Product);

public class GetProductByIdHandler(IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken)
            ?? throw new ProductNotFoundException(query.Id);

        return new GetProductByIdResult(product);
    }
}
