using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.RepositoryContracts;

/// <summary>
/// Represents a repository for managing 'products' table
/// </summary>
public interface IProductsRepository
{
    /// <summary>
    /// Retrieves all products asynchronously.
    /// </summary>
    /// <returns>The task result contains an enumerable collection of products. The collection will be empty if no products are available.</returns>
        Task<IEnumerable<Product>> GetProducts();

    /// <summary>
    /// Retrieves all products based on the specified condition asynchronously.
    /// </summary>
    /// <param name="conditionExpression">The condition to filter products</param>
    /// <returns>Returns a collection of matching products</returns>
    Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Retrieves a product based on the specified condition asynchronously.
    /// </summary>
    /// <param name="conditionExpression">The condition to filter products</param>
    /// <returns>Returns the matching product if found; otherwise, null</returns>
    Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Adds a new profuct into the products table asynchronously
    /// </summary>
    /// <param name="product">The product to be added</param>
    /// <returns>The added product with its generated ID if successful; otherwise, null</returns>
    Task<Product?> AddProduct(Product product);

    /// <summary>
    /// Updates an existing product in the products table asynchronously
    /// </summary>
    /// <param name="product">The product to be updated</param>
    /// <returns>The updated product if successful; otherwise, null</returns>
    Task<Product?> UpdateProduct(Product product);
    /// <summary>
    /// Deletes a product from the products table asynchronously
    /// </summary>
    /// <param name="productID">The productID of the product to be deleted</param>
    /// <returns>Returns a boolean indicating whether the deletion was successful</returns>
    Task<bool> DeleteProduct(Guid productID);
}
