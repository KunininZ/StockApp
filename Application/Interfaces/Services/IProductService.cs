using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel,Product>
    {
        Task<List<ProductViewModel>> GetAllViewModelWithInclude();
        Task<List<ProductViewModel>> GetAllViewModelWithFilters(FilterProductViewModel filters);
    }
}
