using Microsoft.EntityFrameworkCore;
using Inventory.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
        b => b.MigrationsAssembly("Inventory.Infrastructure")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<Inventory.Application.Interface.ITaxRepository, Inventory.Infrastructure.Repositories.TaxRepository>();
builder.Services.AddScoped<Inventory.Application.Service.ITaxService, Inventory.Application.Service.TaxService>();

builder.Services.AddControllers();

// Standardize model validation errors to our custom ApiResponse wrapper
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(kvp => kvp.Value.Errors.Select(e => e.ErrorMessage))
            .ToList();

        var message = "Validation Failed: " + string.Join(" | ", errors);
        
        var response = new Inventory.Application.DTOS.ApiResponse(false, message);
        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(response);
    };
});

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

// Automatically seed Taxes and initial data if not present
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        if (!context.Taxes.Any())
        {
            context.Taxes.AddRange(
                new Inventory.Domain.Model.Tax { Name = "GST 5%", Percentage = 5.00m, IsActive = true, IsCompound = false },
                new Inventory.Domain.Model.Tax { Name = "GST 12%", Percentage = 12.00m, IsActive = true, IsCompound = false },
                new Inventory.Domain.Model.Tax { Name = "GST 18%", Percentage = 18.00m, IsActive = true, IsCompound = false },
                new Inventory.Domain.Model.Tax { Name = "GST 28%", Percentage = 28.00m, IsActive = true, IsCompound = false },
                new Inventory.Domain.Model.Tax { Name = "Standard VAT 10%", Percentage = 10.00m, IsActive = true, IsCompound = false }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database seeding notice: {ex.Message}");
    }
}

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
