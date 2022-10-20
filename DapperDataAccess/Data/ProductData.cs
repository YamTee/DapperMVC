using Dapper;
using DapperDataAccess.DataAccess;
using DapperModels;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DapperDataAccess.Data;

public interface IProductData
{
	Task<Product?> GetProduct(int productId);
	Task<IEnumerable<Product>> GetProducts();
	Task<IEnumerable<Product>> GetProductByJoin();
	Task<IEnumerable<ProductCategory>> GetAllProductCategories();
	Task<IEnumerable<ProductModel>> GetAllProductModels();
}

public class ProductData : IProductData
{
	public readonly IBaseDataAccess _db;
    private readonly IConfiguration _config;

    public ProductData(IBaseDataAccess db, IConfiguration config)
	{
		_db = db;
		_config = config;
	}

	public Task<IEnumerable<Product>> GetProducts() =>
		_db.LoadData<Product, dynamic>("GetAllProducts", new { });

	public async Task<Product?> GetProduct(int productId)
	{
		var results = await _db.LoadData<Product, dynamic>(
			"GetProductByID",
			new { ProductID = productId });
		return results.FirstOrDefault();
	}

	public async Task<IEnumerable<Product>> GetProductByJoin()
	{
		using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

		return await connection.QueryAsync<Product, ProductCategory, ProductModel, Product>(
            "GetAllProducts",
			map: (product, category, model) =>
			{
				product.ProductCategory = category;
				product.ProductModel = model;
				return product;
			},
			param: new {},
			splitOn: "ProductCategoryID,ProductModelID");
	}

	public async Task<IEnumerable<ProductCategory>> GetAllProductCategories()
	{
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

        var productCategoryDictionary = new Dictionary<int, ProductCategory>();

		return await connection.QueryAsync<ProductCategory, Product, ProductCategory>(
			"GetAllProductCategories",
			(productCategory, product) =>
			{
				ProductCategory productCategoryEntry;

				if (!productCategoryDictionary.TryGetValue(productCategory.ProductCategoryID, out productCategoryEntry))
				{
					productCategoryEntry = productCategory;
					productCategoryEntry.Products = new List<Product>();
					productCategoryDictionary.Add(productCategoryEntry.ProductCategoryID, productCategoryEntry);
				}

				productCategoryEntry.Products.Add(product);
				return productCategoryEntry;
			},
			param: new { },
			splitOn: "ProductID");
    }

	public async Task<IEnumerable<ProductModel>> GetAllProductModels()
	{
		using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));

		var productModelDictionary = new Dictionary<int, ProductModel>();

		return await connection.QueryAsync<ProductModel, Product, ProductModel>(
			"GetAllProductModels",
			(productModel, product) =>
			{
				ProductModel productModelEntry;

				if (!productModelDictionary.TryGetValue(productModel.ProductModelID, out productModelEntry))
				{
					productModelEntry = productModel;
					productModelEntry.Products = new List<Product>();
					productModelDictionary.Add(productModel.ProductModelID, productModelEntry);
				}

				productModelEntry.Products.Add(product);
				return productModelEntry;
			},
			param: new { },
			splitOn: "ProductID");
	}

}
