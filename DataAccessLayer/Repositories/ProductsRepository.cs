using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> AddProduct(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == productID);

        if (product == null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        int rows = await _dbContext.SaveChangesAsync();

        return rows > 0;
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await _dbContext.Products.FirstOrDefaultAsync(conditionExpression);

        return product;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        IEnumerable<Product> products = await _dbContext.Products.ToListAsync();

        return products;
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await _dbContext.Products.Where(conditionExpression).ToListAsync();
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.ProductName = product.ProductName;
        existingProduct.Category = product.Category;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.QuantityInStock = product.QuantityInStock;

        int rows = await _dbContext.SaveChangesAsync();

        return existingProduct;
    }
}
