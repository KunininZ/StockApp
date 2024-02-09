using AutoMapper;
using Microsoft.AspNetCore.Http;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.User;
using StockApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : GenericService<SaveCategoryViewModel, CategoryViewModel, Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly AuthenticationResponse userViewModel;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) :base(categoryRepository, mapper)
        {
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }       

        public async Task<List<CategoryViewModel>> GetAllViewModelWithInclude()
        {
            var categoryList = await _categoryRepository.GetAllWithIncludeAsync(new List<string> { "Products"});

            return categoryList.Select(category => new CategoryViewModel
            {
                Name = category.Name,
                Description = category.Description,
                Id= category.Id,
                ProductsQuantity = category.Products.Where(product=> product.UserId == userViewModel.Id).Count()
            }).ToList();
        }

    }
}
