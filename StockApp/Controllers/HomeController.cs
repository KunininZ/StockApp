using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Products;
using StockApp.Infrastructure.Persistence.Contexts;
using System.Threading.Tasks;
using WebApp.StockApp.Middlewares;

namespace StockApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;  

        public HomeController(IProductService productService, ICategoryService categoryService,ValidateUserSession validateUserSession)
        {
            _productService = productService;
            _categoryService = categoryService;           
        }

        public async Task<IActionResult> Index(FilterProductViewModel vm)
        { 
            ViewBag.Categories = await _categoryService.GetAllViewModel();
            return View(await _productService.GetAllViewModelWithFilters(vm));
        }

    }
}
