using ErpEssentials.Stock.Api.Middlewares;
using ErpEssentials.Stock.Application;
using ErpEssentials.Stock.Application.Abstractions.Catalogs.Brands;
using ErpEssentials.Stock.Application.Abstractions.Catalogs.Categories;
using ErpEssentials.Stock.Application.Abstractions.Products;
using ErpEssentials.Stock.Application.Abstractions.Products.Lots;
using ErpEssentials.Stock.Application.Behaviors;
using ErpEssentials.Stock.Domain.Catalogs.Brands;
using ErpEssentials.Stock.Domain.Catalogs.Categories;
using ErpEssentials.Stock.Domain.Products;
using ErpEssentials.Stock.Domain.Products.Lots;
using ErpEssentials.Stock.Infrastructure.Persistence;
using ErpEssentials.Stock.Infrastructure.Persistence.Queries;
using ErpEssentials.Stock.Infrastructure.Persistence.Repositories;
using ErpEssentials.Stock.SharedKernel.Abstractions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers with camelCase JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR with ValidationBehavior
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Repositories and Queries
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

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ERP Essentials API", Version = "v1" });

    // Group endpoints by ApiExplorerSettings GroupName
    c.TagActionsBy(api => new[] { api.GroupName ?? "Default" });
    c.DocInclusionPredicate((_, api) => true);
});

var app = builder.Build();

// HTTPS redirection
app.UseHttpsRedirection();

// Global exception handling middleware
app.UseMiddleware<UnexpectedExceptionMiddleware>();

// Swagger UI (only in development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP Essentials API v1");
    });
}

// Map controllers
app.MapControllers();

app.Run();
