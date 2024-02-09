using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Application.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }
        public string UserId { get; set; }   
        public string CategoryName { get; set; }
    }
}
