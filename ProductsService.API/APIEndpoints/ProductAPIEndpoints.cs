using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;

namespace ProductsService.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        // GET /api/products
        app.MapGet("/api/products", async (IProductsService productsService) => { 
            List<ProductResponse?> products = await productsService.GetProducts();

            return Results.Ok(products);
        });

        // GET /api/products/search/product-id/{productID}
        app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (IProductsService productsService, Guid ProductID) => {
            ProductResponse? product = await productsService.GetProductbyCondition(p => p.ProductID == ProductID);

            return Results.Ok(product);
        });

        //GET /api/products/search/xxxxxxxxxxxxxxxxxx
        app.MapGet("/api/products/search/{SearchString}", async (IProductsService productsService, string SearchString) =>
        {
            Console.WriteLine($"Debug =====: {SearchString}");
            List<ProductResponse?> productsByProductName = await productsService.GetProductsbyCondition(temp => temp.ProductName != null && temp.ProductName.Contains(SearchString));

            List<ProductResponse?> productsByCategory = await productsService.GetProductsbyCondition(temp => temp.Category != null && temp.Category.Contains(SearchString));

            var products = productsByProductName.Union(productsByCategory);

            return Results.Ok(products);
        });


        //POST /api/products
        app.MapPost("/api/products", async (IProductsService productsService, IValidator<ProductAddRequest> productAddRequestValidator, ProductAddRequest productAddRequest) =>
        {
            //Validate the ProductAddRequest object using Fluent Validation
            ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

            //Check the validation result
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                  .GroupBy(temp => temp.PropertyName)
                  .ToDictionary(grp => grp.Key,
                    grp => grp.Select(err => err.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }


            ProductResponse? addedProductResponse = await productsService.AddProduct(productAddRequest);
            if (addedProductResponse == null)
            {
                return Results.Problem("Error in adding product");
            }

            return Results.Created($"/api/products/search/product-id/{addedProductResponse.ProductID}", addedProductResponse);
        });


        //PUT /api/products
        app.MapPut("/api/products", async (IProductsService productsService, IValidator<ProductUpdateRequest> productUpdateRequestValidator, ProductUpdateRequest productUpdateRequest) =>
        {
            //Validate the ProductUpdateRequest object using Fluent Validation
            ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

            //Check the validation result
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                  .GroupBy(temp => temp.PropertyName)
                  .ToDictionary(grp => grp.Key,
                    grp => grp.Select(err => err.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }


            ProductResponse? updatedProductResponse = await productsService.UpdateProduct(productUpdateRequest);

            if (updatedProductResponse == null)
            {
                return Results.Problem("Error in updating product");
            }

            return Results.Ok(updatedProductResponse);
        });


        //DELETE /api/products/xxxxxxxxxxxxxxxxxxx
        app.MapDelete("/api/products/{ProductID:guid}", async (IProductsService productsService, Guid ProductID) =>
        {
            bool isDeleted = await productsService.DeleteProduct(ProductID);

            if (isDeleted)
                return Results.Ok(true);
            else
                return Results.Problem("Error in deleting product");
        });

        return app;
    }
}
