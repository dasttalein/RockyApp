using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RockyApp.Models.ViewModels
{
    public class ProductVM
    {
        public IEnumerable<SelectListItem> CategoryDropDown { get; set; }
        public Product Product { get; set; }
    }
}