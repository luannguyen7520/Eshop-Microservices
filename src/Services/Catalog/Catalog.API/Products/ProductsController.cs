using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products;

[ApiController]
[Route("api/[controller]")]
public partial class ProductsController(ISender sender) : ControllerBase
{
}
