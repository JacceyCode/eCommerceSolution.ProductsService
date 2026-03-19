using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services;

public class ProductsService : IProductsService
{
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductsRepository _productsRepository;

    public ProductsService(
        IValidator<ProductAddRequest> productAddRequestValidator,
        IValidator<ProductUpdateRequest> productUpdateRequestValidator, 
        IMapper mapper, 
        IProductsRepository productsRepository)
    {
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _mapper = mapper;
        _productsRepository = productsRepository;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        // Validate request using FluentValidation
        ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(e => e.PropertyName + ": " + e.ErrorMessage));

            throw new ArgumentException(errors);
        }

        Product productInput = _mapper.Map<Product>(productAddRequest);

        Product? addedProduct = await _productsRepository.AddProduct(productInput);

        if (addedProduct == null)
        {
            return null;
        }

        return _mapper.Map<ProductResponse>(addedProduct);
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        //Product? existingProduct = await _productsRepository.GetProductByCondition(p => p.ProductID == productID);

        //if (existingProduct == null)
        //{
        //    return false;
        //}

        bool isDeleted = await _productsRepository.DeleteProduct(productID);

        return isDeleted;
    }

    public async Task<ProductResponse?> GetProductbyCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        Product? product = await _productsRepository.GetProductByCondition(conditionExpression);

        if (product == null)
        {
            return null;
        }

        return _mapper.Map<ProductResponse>(product);
    }

    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = await _productsRepository.GetProducts();

        IEnumerable<ProductResponse?> productsResponse = _mapper.Map<IEnumerable<ProductResponse>>(products);

        return productsResponse.ToList();

    }

    public async Task<List<ProductResponse?>> GetProductsbyCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        IEnumerable<Product?> products = await _productsRepository.GetProductsByCondition(conditionExpression);

        IEnumerable<ProductResponse?> productsResponse = _mapper.Map<IEnumerable<ProductResponse?>>(products);

        return productsResponse.ToList();
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        // Check for existing product
        Product? existingProduct = await _productsRepository.GetProductByCondition(p => p.ProductID == productUpdateRequest.ProductID);

        if (existingProduct == null)
        {
            throw new ArgumentException("Invalid Product ID");
        }

        // Validate request using FluentValidation
        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(e => e.PropertyName + ": " + e.ErrorMessage));

            throw new ArgumentException(errors);
        }

        Product productInput = _mapper.Map<Product>(productUpdateRequest);

        Product? updatedProduct = await _productsRepository.UpdateProduct(productInput);

        if (updatedProduct == null)
        {
            return null;
        }

        return _mapper.Map<ProductResponse>(updatedProduct);
    }
}
