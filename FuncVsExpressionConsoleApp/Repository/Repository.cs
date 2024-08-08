using System.Linq.Expressions;
using FuncVsExpressionConsoleApp.Data;
using System.Collections.Generic;

namespace FuncVsExpressionConsoleApp.Repository;

public class  Repository<T> where T : class
{
    // public IEnumerable<T> GetWithFunc(Func<T, bool> conditionLambda)
    // {
    //     using (var db = new AppDbContext())
    //     {
    //         return db.Set<T>().ToList().Where(conditionLambda).ToList();
    //     }
    // }
    //
    // public IEnumerable<T> GetWithExpression(Expression<Func<T, bool>> conditionLambda)
    // {
    //     using (var db = new AppDbContext())
    //     {
    //         return db.Set<T>().Where(conditionLambda).ToList();
    //     }
    // }
    
    //Function to remove all records from the table
    public void RemoveAll()
    {
        using (var db = new AppDbContext())
        {
            db.Set<T>().RemoveRange(db.Set<T>());
            db.SaveChanges();
        }
    }
    
    //Add Data to the table
    public void Add(IEnumerable<T> item)
    {
        using (var db = new AppDbContext())
        {
            db.Set<T>().AddRange(item);
            db.SaveChanges();
        }
    }
    
    public void BulkInsertWithFunc(IEnumerable<T> items, Func<T, bool> conditionLambda)
    {
        using (var db = new AppDbContext())
        {
            // Ensure unique items by primary key
            var filteredItems = items.Where(conditionLambda).ToList();
            db.Set<T>().AddRange(filteredItems);
            db.SaveChanges();
        }
    }

    public void BulkInsertWithExpression(IEnumerable<T> items, Expression<Func<T, bool>> conditionLambda)
    {
        using (var db = new AppDbContext())
        {
            var compiledExpression = conditionLambda.Compile();
            var filteredItems = items.Where(compiledExpression).ToList();
            db.Set<T>().AddRange(filteredItems);
            db.SaveChanges();
        }
    }
}