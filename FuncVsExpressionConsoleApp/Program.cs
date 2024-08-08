using System.Linq.Expressions;
using System.Diagnostics;
using System;
using System.Linq;
using FuncVsExpressionConsoleApp.Data;
using FuncVsExpressionConsoleApp.Models;
using FuncVsExpressionConsoleApp.Repository;

namespace FuncVsExpressionConsoleApp
{
   

    static class Program
    {
        static void Main(string[] args)
        {
            var repository = new Repository<Product>();
            var stopwatch = new Stopwatch();

            // Generate test data
            var items = GenerateTestData(5000000);

            //Direct insert
            stopwatch.Start();
            IEnumerable<Product> products = items as Product[] ?? items.ToArray();
            repository.Add(products);
            stopwatch.Stop();
            Console.WriteLine($"Time taken for bulk insert direct: {stopwatch.ElapsedMilliseconds} ms");
            repository.RemoveAll();
            
            // Measure performance for Func-based bulk insert
            stopwatch.Restart();
            IEnumerable<Product> enumerable = products.ToList();
            repository.BulkInsertWithFunc(enumerable, IsExpensiveFunc());
            stopwatch.Stop();
            Console.WriteLine($"Time taken for bulk insert with Func filtering: {stopwatch.ElapsedMilliseconds} ms");

            // Measure performance for Expression-based bulk insert
            repository.RemoveAll();
            stopwatch.Restart();
            repository.BulkInsertWithExpression(enumerable, IsExpensiveExpression());
            stopwatch.Stop();
            Console.WriteLine($"Time taken for bulk insert with Expression filtering: {stopwatch.ElapsedMilliseconds} ms");
        }

        static IEnumerable<Product> GenerateTestData(int count)
        {
            // Use a HashSet to avoid duplicate records
            var products = new HashSet<Product>();
            for (int i = 1; i <= count; i++)
            {
                products.Add(new Product { Name = $"Product {i}", Price = i });
            }
            return products;
        }

        private static Func<Product, bool> IsExpensiveFunc()
        {
            return product => product.Price > 50000;
        }

        private static Expression<Func<Product, bool>> IsExpensiveExpression()
        {
            return product => product.Price > 50000;
        }
    }
}
