namespace HedgeHogOtter.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<HedgeHogOtter.Models.HedgeHogOtterContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(HedgeHogOtter.Models.HedgeHogOtterContext context)
        {
            
            context.Books.AddOrUpdate(
                b => b.Title,
                new Book { Title = "The Fellowship of the Ring", Author = "J.R.R. Tolkien", Description = "The first book of the Lord of the Rings trilogy.", Price = 14.99, FeatureFlag = 1, Quantity = 1, ISBN = "9780007129706" },
                new Book { Title = "The Two Towers", Author = "J.R.R. Tolkien", Description = "The second book of the Lord of the Rings trilogy.", Price = 24.99, FeatureFlag = 1, Quantity = 2, ISBN = "9780007129713" },
                new Book { Title = "The Return of the King", Author = "J.R.R. Tolkien", Description = "The third book of the Song of Ice and Fire series.", Price = 34.99, FeatureFlag = 1, Quantity = 3, ISBN = "9780007129720" },
                new Book { Title = "A Game of Thrones", Author = "George R.R. Martin", Description = "The first book of the Song of Ice and Fire series.", Price = 4.99, FeatureFlag = 0, Quantity = 12, ISBN = "9780553573404" },
                new Book { Title = "A Clash of Kings", Author = "George R.R. Martin", Description = "The second book of the Song of Ice and Fire series.", Price = 5.99, FeatureFlag = 0, Quantity = 8, ISBN = "9780553579901" },
                new Book { Title = "A Storm of Swords", Author = "George R.R. Martin", Description = "The third book of the Song of Ice and Fire series.", Price = 6.99, FeatureFlag = 0, Quantity = 4, ISBN = "9780553573428" },
                new Book { Title = "A Feast for Crows", Author = "George R.R. Martin", Description = "The fourth book of the Song of Ice and Fire series.", Price = 7.99, FeatureFlag = 0, Quantity = 17, ISBN = "9780553582024" },
                new Book { Title = "A Dance With Dragons", Author = "George R.R. Martin", Description = "The fifth book of the Song of Ice and Fire series.", Price = 8.99, FeatureFlag = 0, Quantity = 6, ISBN = "9780553801477" }
            );
            context.Users.AddOrUpdate(
                b => b.Username,
                new User { Username = "Admin", Email = "admin@test.com", Firstname = "Phil", Lastname = "Kaveny" }
            );

            context.SaveChanges();
        }
    }
}
