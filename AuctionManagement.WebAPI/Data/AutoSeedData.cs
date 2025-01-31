using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using System;

namespace AuctionManagement.WebAPI.Data {

    /// <summary>
    /// Handles the seeding and resetting of the database.
    /// This class is responsible for populating the database with initial data and resetting the database.
    /// </summary>
    public class AutoSeedData {

        /// <summary>
        /// The database context for the auction management system.
        /// </summary>
        private readonly AuctionContext auctionContext;


        /// <summary>
        /// Initializes a new instance of the AutoSeedData class.
        /// </summary>
        /// <param name="auctionContext"></param>
        public AutoSeedData(AuctionContext auctionContext) {
            this.auctionContext = auctionContext;
        }


        /// <summary>
        /// Seeds the database with initial data.
        /// This method checks if the database is empty, and if so, populates it with categories, items, and sales.
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
                    new Item { Name = "Laptop HP 15", Price = 800m, Status = Status.Available, CategoryId = 1 },
                    new Item { Name = "Washing Machine LG", Price = 400.50m, Status = Status.Available, CategoryId = 2 },
                    new Item { Name = "Sofa Set", Price = 1110.00m, Status = Status.Available, CategoryId = 3 },
                    new Item { Name = "Nike Running Shoes", Price = 95.00m, Status = Status.Available, CategoryId = 4 },
                    new Item { Name = "Harry Potter Book Set", Price = 178.99m, Status = Status.Available, CategoryId = 5 },
                    new Item { Name = "Gold Necklace", Price = 999.00m, Status = Status.Available, CategoryId = 8 },
                    new Item { Name = "Framed Painting", Price = 300.00m, Status = Status.Available, CategoryId = 9 },
                    new Item { Name = "Antique Watch", Price = 4230.00m, Status = Status.Available, CategoryId = 10 },
                    // Available items
                    new Item { Name = "Electric Kettle", Price = 40.00m, Status = Status.Available, CategoryId = 2 },
                    new Item { Name = "Wardrobe", Price = 500.00m, Status = Status.Available, CategoryId = 3 },
                    new Item { Name = "Sweater", Price = 60.00m, Status = Status.Available, CategoryId = 4 },
                    new Item { Name = "Cookbook", Price = 25.00m, Status = Status.Available, CategoryId = 5 },
                    new Item { Name = "Silver Ring", Price = 150.00m, Status = Status.Available, CategoryId = 6 },
                    new Item { Name = "Antique Chair", Price = 900.00m, Status = Status.Available, CategoryId = 8 } 
        
                };

                auctionContext.Items.AddRange(items);
                auctionContext.SaveChanges();
            }



            if (!auctionContext.Sales.Any()) {
                var sales = new List<Sale>
                {
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), Price = 899.99m, ItemId = 1 },
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), Price = 450.50m, ItemId = 2 },
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)), Price = 1299.00m, ItemId = 3 },
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-20)), Price = 120.00m, ItemId = 4 },
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-25)), Price = 199.99m, ItemId = 5 },
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-8)), Price = 1200.00m, ItemId = 8 },
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-12)), Price = 350.00m, ItemId = 9 },
                    new Sale { Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-18)), Price = 4500.00m, ItemId = 10 }
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
        /// Resets the database.
        /// This method removes all records from the database, ensures the database is created, and then seeds it with initial data.
        /// </summary>
        public void ResetDatabase() {
            auctionContext.Database.EnsureDeleted();
            auctionContext.Database.EnsureCreated();

            SeedData();
        }
    }
}
