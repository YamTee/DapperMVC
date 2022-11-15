namespace DapperMVC.Controllers;

public class ProductController : Controller
{
    private readonly IProductData _iProductData;

    public ProductController(IProductData productData)
    {
        _iProductData = productData;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _iProductData.GetProductByJoin();

        return View(products);
    }

    [HttpGet]
    public IActionResult DataTable()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ServerSide()
    {
        var products = await _iProductData.GetProductByJoin();

        return Json(new { Data = products });
    }

    [HttpGet]
    public async Task<IActionResult> ServerSideProduct()
    {
        var productCategories = await _iProductData.GetAllProductCategories();

        var productCategory = productCategories.Distinct().ToList();

        return Json(new { Data = productCategory });
    }

    [HttpGet]
    public async Task<IActionResult> ServerSideProductModel()
    {
        var productModels = await _iProductData.GetAllProductModels();

        var productModel = productModels.Distinct().ToList();

        return Json(new { Data = productModel });
    }
}
