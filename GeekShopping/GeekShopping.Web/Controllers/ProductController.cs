using GeekShopping.Web.Models;
using GeekShopping.Web.Services;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult ProductCreate()
        {
            return View();
        }

        public async Task<IActionResult> ProductUpdate(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var model = await _productService.FindProductById(id, accessToken);
            if (model != null) return View(model);

            return NotFound();
        }

        [Authorize]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var model = await _productService.FindProductById(id, accessToken);
            if (model != null) return View(model);

            return NotFound();
        }


        [Authorize]
        public async Task<IActionResult> ProductIndex()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var products = await _productService.FindAllProducts(accessToken);

            return View(products);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProductCreate(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.CreateProduct(productModel, accessToken);
                if (response != null) return RedirectToAction(nameof(ProductIndex));
            }

            return View(productModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProductUpdate(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.UpdateProduct(productModel, accessToken);
                if (response != null) return RedirectToAction(nameof(ProductIndex));
            }

            return View(productModel);
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ProductDelete(ProductModel productModel)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.DeleteProductById(productModel.Id, accessToken);
            if (response) return RedirectToAction(nameof(ProductIndex));

            return View(productModel);
        }


    }
}
