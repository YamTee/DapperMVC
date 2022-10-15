using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Dapper.Controllers;

public class ProductController : Controller
{
    private readonly IProductData _IProductData;

    public ProductController(IProductData IProductData)
    {
        _IProductData = IProductData;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _IProductData.GetProductByJoin();

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
        var products = await _IProductData.GetProductByJoin();

        return Json(new {Data = products});
    }
}
