using Microsoft.EntityFrameworkCore;
using product.Application.Interfaces;
using product.Application.UseCases.Commands.CreateProduct;
using product.Infrastructure;
using product.Infrastructure.Repositories;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Product_";
});

builder.Services.AddScoped<IProductReadDbContext>(provider => 
    provider.GetRequiredService<ProductDbContext>());

/*builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<UpdateProductHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddScoped<DeleteProductHandler>();*/

builder.Services.Scan(scan => scan
    .FromAssemblyOf<CreateProductHandler>()
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Handler")))
    .AsSelf()
    .WithScopedLifetime());

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}