using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperModels;

public class ProductModel
{
    public ProductModel()
    {
        this.Products = new HashSet<Product>();
    }

    public int ProductModelID { get; set; }
    [Display(Name = "Product Model")]
    public string ProductModelName { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; }
}

public class ProductCategory
{
    public ProductCategory()
    {
        this.Products = new HashSet<Product>();
    }
    public int ProductCategoryID { get; set; }
    [Display(Name = "Product Category")]
    public string ProductCategoryName { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; }
}

public class Product
{
    public Product()
    {
        this.ProductCategory = new ProductCategory();
        this.ProductModel = new ProductModel();
    }

    [Key]
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal StandardCost{ get; set; }
    public decimal ListPrice{ get; set; }
    public string Size { get; set; } = string.Empty;
    public decimal Weight{ get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime SellStartDate{ get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime SellEndDate{ get; set; }

    [ForeignKey("ProductCategory")]
    public int ProductCategoryID { get; set; }
    public virtual ProductCategory ProductCategory { get; set; }

    [ForeignKey("ProductModel")]
    public int ProductModelID { get; set; }
    public virtual ProductModel ProductModel { get; set; }
}