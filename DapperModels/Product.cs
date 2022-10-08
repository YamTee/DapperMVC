using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperModels;

public class ProductModel
{
    public int ProductModelID { get; set; }
    public string Name { get; set; }
}

public class ProductCategory
{
    public int ProductCategoryID { get; set; }
    public string Name { get; set; }
}

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public string ProductNumber { get; set; }
    public string Color { get; set; }
    public decimal StandardCost{ get; set; }
    public decimal ListPrice{ get; set; }
    public string Size{ get; set; }
    public decimal Weight{ get; set; }
    public DateOnly SellStartDate{ get; set; }
    public DateOnly SellEndDate{ get; set; }
    public virtual ProductCategory ProductCategory { get; set; }
    public virtual ProductModel ProductModel { get; set; }
}