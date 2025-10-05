using ErpEssentials.Application;
using ErpEssentials.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Application.Abstractions.Products;
using ErpEssentials.Application.Abstractions.Products.Lots;
using ErpEssentials.Application.Behaviors;
using ErpEssentials.Domain.Catalogs.Brands;
using ErpEssentials.Domain.Catalogs.Categories;
using ErpEssentials.Domain.Products;
using ErpEssentials.Domain.Products.Lots;
using ErpEssentials.Infrastructure.Persistence;
using ErpEssentials.Infrastructure.Persistence.Queries;
using ErpEssentials.Infrastructure.Persistence.Repositories;
using ErpEssentials.SharedKernel.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers com camelCase para frontend
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR com ValidationBehavior
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Repositories e Queries
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILotRepository, LotRepository>();

builder.Services.AddScoped<IProductQueries, ProductQueries>();
builder.Services.AddScoped<IBrandQueries, BrandQueries>();
builder.Services.AddScoped<ICategoryQueries, CategoryQueries>();
builder.Services.AddScoped<ILotQueries, LotQueries>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
