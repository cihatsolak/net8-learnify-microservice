namespace Learnify.Discount.API.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Discount> Discounts { get; set; } 

    public static AppDbContext Create(IMongoDatabase database)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<AppDbContext>().UseMongoDB(database.Client,
                database.DatabaseNamespace.DatabaseName);

        return new AppDbContext(optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
