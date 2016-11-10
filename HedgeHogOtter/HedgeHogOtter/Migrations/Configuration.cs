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
                new Book { Title = "OS is a Pile", Author = "Kronny", Description = "I wouldn't advise taking it. It blows.", Price = 24.99, FeatureFlag = 0, Quantity = 1},
                new Book { Title = "Pokemon Go Survival Guide", Author = "Nick Dopkins", Description = "The trick is to not get hit by cars and stuff.", Price = 19.99, FeatureFlag = 0, Quantity = 13 },
                new Book { Title = "Dick Nopkins Autobiography", Author = "Dick Nopkins", Description = "Some people are just greater than others.", Price = 99.99, FeatureFlag = 1, Quantity = 21 },
                new Book { Title = "A Game of Thrones", Author = "George R.R. Martin", Description = "People die.", Price = 11.99, FeatureFlag = 1, Quantity = 5 },
                new Book { Title = "Where the Sidewalk Ends", Author = "Shel Silverstein", Description = "Funny poems for kids.", Price = 15.99, FeatureFlag = 0, Quantity = 9 },
                new Book { Title = "Memoirs of a Cincinnati Zoo Gorilla", Author = "Anonymous", Description = "He dies at the end.", Price = 49.99, FeatureFlag = 1, Quantity = 45 },
                new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Description = "Bilbo helps slay a dragon and gets into trouble with some dwarves.", Price = 24.99, FeatureFlag = 0, Quantity = 134 }
            );
            context.Users.AddOrUpdate(
                b => b.Username,
                new User { Username = "Admin", Email = "admin@test.com", Firstname = "Phil", Lastname = "Kaveny"}    
            );

            context.SaveChanges();
        }
    }
}
