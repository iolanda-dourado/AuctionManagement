using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using AuctionManagement.WebAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AuctionContext>(options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("AuctionHouseDB"))
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();


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
