using DapperDataAccess.DataAccess;
using DapperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDataAccess.Data;

public interface IProductData
{
	Task<Product?> GetProduct(int productId);
	Task<IEnumerable<Product>> GetProducts();
}

public class ProductData : IProductData
{
	public readonly IBaseDataAccess _db;

	public ProductData(IBaseDataAccess db)
	{
		_db = db;
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
}
