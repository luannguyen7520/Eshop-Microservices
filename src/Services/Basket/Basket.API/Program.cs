using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Application Services
builder.Services.AddControllers();
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(Program).Assembly);
    options.AddOpenBehavior(typeof(ValidationBehavior<,>));
    options.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
#endregion

#region Database Services
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(s => s.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
#endregion

#region Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
   options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
   var handler = new HttpClientHandler
   {
       ServerCertificateCustomValidationCallback =
       HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
   };
   return handler;
});
#endregion

#region Cross-Cutting Services
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API");
        options.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler(options => { });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
