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

}
