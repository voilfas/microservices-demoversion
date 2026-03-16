using Microsoft.EntityFrameworkCore;
using product.Application;
using product.Application.UseCases.CreateProduct;
using product.Application.UseCases.DeleteProduct;
using product.Application.UseCases.GetProductById;
using product.Application.UseCases.UpdateProduct;
using product.Infrastructure;
using product.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();