using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using System;

namespace AuctionManagement.WebAPI.Data {
    public class AutoSeedData {

        private readonly AuctionContext auctionContext;

        public AutoSeedData(AuctionContext auctionContext) {
            this.auctionContext = auctionContext;
        }


        /// <summary>
        /// Method to seed the database with initial data
        /// </summary>
        public void SeedData() {
            if (!auctionContext.Categories.Any()) {

                var categories = new List<Category>
                {
                    new Category { Description = "Electronics" },
                    new Category { Description = "Home Appliances" },
                    new Category { Description = "Furniture" },
                    new Category { Description = "Clothing" },
                    new Category { Description = "Books" },
                    new Category { Description = "Jewelry" },
                    new Category { Description = "Art" },
                    new Category { Description = "Antiques" },
                    // Categories without items
                    new Category { Description = "Toys" },
                    new Category { Description = "Sports" },
                    new Category { Description = "Music" },
                    new Category { Description = "Vehicles" },
                    new Category { Description = "Garden" }
                };

                auctionContext.Categories.AddRange(categories);
                auctionContext.SaveChanges();
            }

            if (!auctionContext.Items.Any()) {
                var items = new List<Item>
                {
                    new Item { Name = "Laptop HP 15", Price = 800m, Status = Status.Available, CategoryId = 1 },  // Electronics
                    new Item { Name = "Washing Machine LG", Price = 400.50m, Status = Status.Available, CategoryId = 2 },  // Home Appliances
                    new Item { Name = "Sofa Set", Price = 1110.00m, Status = Status.Available, CategoryId = 3 },  // Furniture
                    new Item { Name = "Nike Running Shoes", Price = 95.00m, Status = Status.Available, CategoryId = 4 },  // Clothing
                    new Item { Name = "Harry Potter Book Set", Price = 178.99m, Status = Status.Available, CategoryId = 5 },  // Books
                    new Item { Name = "Gold Necklace", Price = 999.00m, Status = Status.Available, CategoryId = 8 },  // Jewelry
                    new Item { Name = "Framed Painting", Price = 300.00m, Status = Status.Available, CategoryId = 9 },  // Art
                    new Item { Name = "Antique Watch", Price = 4230.00m, Status = Status.Available, CategoryId = 10 },  // Antiques
                    // Available items
                    new Item { Name = "Electric Kettle", Price = 40.00m, Status = Status.Available, CategoryId = 2 },  // Home Appliances
                    new Item { Name = "Wardrobe", Price = 500.00m, Status = Status.Available, CategoryId = 3 },  // Furniture
                    new Item { Name = "Sweater", Price = 60.00m, Status = Status.Available, CategoryId = 4 },  // Clothing
                    new Item { Name = "Cookbook", Price = 25.00m, Status = Status.Available, CategoryId = 5 },  // Books
                    new Item { Name = "Silver Ring", Price = 150.00m, Status = Status.Available, CategoryId = 6 },  // Jewelry
                    new Item { Name = "Antique Chair", Price = 900.00m, Status = Status.Available, CategoryId = 8 }  // Antiques
        
                };

                auctionContext.Items.AddRange(items);
                auctionContext.SaveChanges();
            }



            if (!auctionContext.Sales.Any()) {
                var sales = new List<Sale>
                {
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), Price = 899.99m, ItemId = 1 },  // Laptop HP 15
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), Price = 450.50m, ItemId = 2 },  // Washing Machine LG
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)), Price = 1299.00m, ItemId = 3 },  // Sofa Set
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-20)), Price = 120.00m, ItemId = 4 },  // Nike Running Shoes
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-25)), Price = 199.99m, ItemId = 5 },  // Harry Potter Book Set
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-8)), Price = 1200.00m, ItemId = 8 },  // Gold Necklace
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-12)), Price = 350.00m, ItemId = 9 },  // Framed Painting
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-18)), Price = 4500.00m, ItemId = 10 }  // Antique Watch
                };

                var items = auctionContext.Items.ToList();
                var item = new Item();
                foreach (var sale in sales) {
                    item = auctionContext.Items.Find(sale.ItemId);
                    item.Status = Status.Sold;
                }

                auctionContext.Sales.AddRange(sales);
                auctionContext.SaveChanges();
            }
        }



        /// <summary>
        /// Method
        /// </summary>
        public void ResetDatabase() {
            // Remove todos os registros antes de reiniciar a base de dados
            auctionContext.Database.EnsureDeleted();
            auctionContext.Database.EnsureCreated();

            // Popula com dados iniciais
            SeedData();
        }
    }
}
