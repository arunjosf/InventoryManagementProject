using Microsoft.EntityFrameworkCore;
using Inventory.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
        b => b.MigrationsAssembly("Inventory.Infrastructure")));
// Add services to the container.
builder.Services.AddScoped<Inventory.Application.Interface.IPurchaseRepository, Inventory.Infrastructure.Repositories.PurchaseRepository>();
builder.Services.AddScoped<Inventory.Application.Interface.IInventoryRepository, Inventory.Infrastructure.Repositories.InventoryRepository>();
builder.Services.AddScoped<Inventory.Application.Interface.IProductRepository, Inventory.Infrastructure.Repositories.ProductRepository>();
builder.Services.AddScoped<Inventory.Application.Interface.ISupplierRepository, Inventory.Infrastructure.Repositories.SupplierRepository>();

builder.Services.AddScoped<Inventory.Application.Interface.IPurchaseService, Inventory.Application.Service.PurchaseService>();
builder.Services.AddScoped<Inventory.Application.Interface.IInventoryService, Inventory.Application.Service.InventoryService>();
builder.Services.AddScoped<Inventory.Application.Interface.IProductService, Inventory.Application.Service.ProductService>();
builder.Services.AddScoped<Inventory.Application.Interface.ISupplierService, Inventory.Application.Service.SupplierService>();
builder.Services.AddScoped<Inventory.Application.Interface.IProductClassificationRepository, Inventory.Infrastructure.Repositories.ProductClassificationRepository>();
builder.Services.AddScoped<Inventory.Application.Service.IProductClassificationService, Inventory.Application.Service.ProductClassificationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

app.UseMiddleware<Inventory.Api.Middleware.ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
