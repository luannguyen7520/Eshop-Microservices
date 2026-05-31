using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket;

[ApiController]
[Route("api/[controller]")]
public partial class BasketController(ISender sender) : ControllerBase
{
}
