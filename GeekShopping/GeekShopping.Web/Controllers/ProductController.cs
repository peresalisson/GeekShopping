using GeekShopping.Web.Models;
using GeekShopping.Web.Services;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService? _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public async Task<IActionResult> ProductIndex()
        {
            if (_productService == null) throw new ArgumentNullException(nameof(_productService));
            var products = await _productService.FindAllProducts();

            return View(products);
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel productModel)
        {
            if (_productService == null) throw new ArgumentNullException(nameof(_productService));
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProduct(productModel);
                if (response != null) return RedirectToAction(nameof(ProductIndex));
            }

            return View(productModel);
        }

        public async Task<IActionResult> ProductUpdate(int id)
        {
            if (_productService == null) throw new ArgumentNullException(nameof(_productService));
            
            var model = await _productService.FindProductById(id);
            if (model != null) return View(model);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductModel productModel)
        {
            if (_productService == null) throw new ArgumentNullException(nameof(_productService));
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProduct(productModel);
                if (response != null) return RedirectToAction(nameof(ProductIndex));
            }

            return View(productModel);
        }

        public async Task<IActionResult> ProductDelete(int id)
        {
            if (_productService == null) throw new ArgumentNullException(nameof(_productService));
            
            var model = await _productService.FindProductById(id);
            if (model != null) return View(model);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductModel productModel)
        {
            if (_productService == null) throw new ArgumentNullException(nameof(_productService));
            var response = await _productService.DeleteProductById(productModel.Id);
            if (response) return RedirectToAction(nameof(ProductIndex));

            return View(productModel);
        }


    }
}
