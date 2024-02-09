using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Products;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.StockApp.Middlewares;

namespace StockApp.Controllers
{
    [Authorize(Roles ="Basic")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService; 

        public ProductController(IProductService productService, ICategoryService categoryService, ValidateUserSession validateUserSession)
        {
            _productService = productService;
            _categoryService = categoryService;       
        }
        public async Task<IActionResult> Index()
        {          
            return View(await _productService.GetAllViewModelWithInclude());
        }

        public async Task<IActionResult> Create()
        {         
            SaveProductViewModel vm = new();
            vm.Categories = await _categoryService.GetAllViewModel();
            return View("SaveProduct", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveProductViewModel vm)
        {           

            if (!ModelState.IsValid)
            {
                vm.Categories = await _categoryService.GetAllViewModel();
                return View("SaveProduct", vm);
            }

            SaveProductViewModel productVm = await _productService.Add(vm);

            if (productVm.Id != 0 && productVm != null)
            {
                productVm.ImageUrl = UploadFile(vm.File, productVm.Id);

                await _productService.Update(productVm,productVm.Id);
            }

            return RedirectToRoute(new { controller = "Product", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {   
            SaveProductViewModel vm = await _productService.GetByIdSaveViewModel(id);
            vm.Categories = await _categoryService.GetAllViewModel();
            return View("SaveProduct", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {               
                vm.Categories = await _categoryService.GetAllViewModel();
                return View("SaveProduct", vm);
            }

            SaveProductViewModel productVm = await _productService.GetByIdSaveViewModel(vm.Id);
            vm.ImageUrl = UploadFile(vm.File, vm.Id, true, productVm.ImageUrl);
            await _productService.Update(vm,vm.Id);
            return RedirectToRoute(new { controller = "Product", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {   
            return View(await _productService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {  
            await _productService.Delete(id);

            string basePath = $"/Images/Products/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo folder in directory.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

            return RedirectToRoute(new { controller = "Product", action = "Index" });
        }

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode)
            {
                if (file == null)
                {
                    return imagePath;
                }
            }
            string basePath = $"/Images/Products/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file extension
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";
        }
    }
}
