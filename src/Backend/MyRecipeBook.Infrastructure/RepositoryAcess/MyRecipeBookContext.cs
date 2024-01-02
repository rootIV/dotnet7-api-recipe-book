using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.RepositoryAcess;

public class MyRecipeBookContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Codes> Codes { get; set; }
    public DbSet<Connection> Connections { get; set; }

    public MyRecipeBookContext(DbContextOptions<MyRecipeBookContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyRecipeBookContext).Assembly);
    }
}
