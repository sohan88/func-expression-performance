using FuncVsExpressionConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FuncVsExpressionConsoleApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("ProductsDb");
    }
}