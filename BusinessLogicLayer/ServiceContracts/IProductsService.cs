using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace BusinessLogicLayer.ServiceContracts;

public interface IProductsService
{
    /// <summary>
    /// Retrieves the list of products from products repository
    /// </summary>
    /// <returns>Returns list of ProductResponse</returns>
    Task<List<ProductResponse?>> GetProducts();

    /// <summary>
    /// Retrieves list of products matching with given condition
    /// </summary>
    /// <param name="conditionExpression">Condition for filter</param>
    /// <returns>Returns a list of matching Products</returns>
    Task<List<ProductResponse?>> GetProductsbyCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Returns a product matching with given condition
    /// </summary>
    /// <param name="conditionExpression">Condition for filter</param>
    /// <returns>Returns a product matching the condition</returns>
    Task<ProductResponse?> GetProductbyCondition(Expression<Func<Product, bool>> conditionExpression);

    /// <summary>
    /// Adds product into table using products repository
    /// </summary>
    /// <param name="productAddRequest">Request object containing product details</param>
    /// <returns>Returns added product or null if error occurs</returns>
    Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);

    /// <summary>
    /// Updates the existing product based on the ProductID using products repository
    /// </summary>
    /// <param name="productUpdateRequest">Request object containing updated product details</param>
    /// <returns>Returns updated product or null if error occurs</returns>
    Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);

    /// <summary>
    /// Deletes an existing product based on given product id using products repository
    /// </summary>
    /// <param name="productID">Product ID</param>
    /// <returns>Returns true if product is deleted successfully, otherwise false</returns>
    Task<bool> DeleteProduct(Guid productID);
}
