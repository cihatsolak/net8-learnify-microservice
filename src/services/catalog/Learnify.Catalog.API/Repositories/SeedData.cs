namespace Learnify.Catalog.API.Repositories;

public static class SeedData
{
    public static async Task AddSeedDataExtAsync(this WebApplication webApplication)
    {
        using AsyncServiceScope asyncServiceScope = webApplication.Services.CreateAsyncScope();

        AppDbContext context = asyncServiceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        if (!await context.Database.CanConnectAsync())
        {
            throw new InvalidOperationException("Cannot connect to database.");
        }

        if (!await context.Categories.AnyAsync())
        {
            List<Category> seedCategories =
            [
                new(){ Id = NewId.NextSequentialGuid(), Name= "Development" },
                new(){ Id = NewId.NextSequentialGuid(), Name= "Business" },
                new(){ Id = NewId.NextSequentialGuid(), Name= "IT & Software" },
                new(){ Id = NewId.NextSequentialGuid(), Name= "Office & Productivity" },
                new(){ Id = NewId.NextSequentialGuid(), Name= "Personel Development" }
            ];

            await context.Categories.AddRangeAsync(seedCategories);
            await context.SaveChangesAsync();
        }

        if (!await context.Courses.AnyAsync())
        {
            var category = await context.Categories.FirstAsync();
            Guid randomUserId = NewId.NextGuid();

            List<Course> courses =
            [
                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "C#",
                    Description = "C# Course",
                    Price = 100,
                    UserId = randomUserId,
                    Created = DateTime.UtcNow,
                    Feature = new Feature { Duration = 10, Rating = 4, EducatorFullName = "Ahmet Yıldız" },
                    CategoryId = category.Id
                },

                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Java",
                    Description = "Java Course",
                    Price = 200,
                    UserId = randomUserId,
                    Created = DateTime.UtcNow,
                    Feature = new Feature { Duration = 10, Rating = 4, EducatorFullName = "Ahmet Yıldız" },
                    CategoryId = category.Id
                },

                new()
                {
                    Id = NewId.NextSequentialGuid(),
                    Name = "Python",
                    Description = "Python Course",
                    Price = 300,
                    UserId = randomUserId,
                    Created = DateTime.UtcNow,
                    Feature = new Feature { Duration = 10, Rating = 4, EducatorFullName = "Ahmet Yıldız" },
                    CategoryId = category.Id
                }
            ];


            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
        }
    }
}
