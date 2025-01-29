using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using AuctionManagement.WebAPI.Services.Interfaces;
using AuctionManagement.WebAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AuctionContext>(options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("AuctionHouseDB"))
    );
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddTransient<ItemsValidator>();
builder.Services.AddTransient<SalesValidator>();
builder.Services.AddTransient<CategoriesValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
