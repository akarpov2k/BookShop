using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Shared.PresentationModels
{
    public class PageItems<T>
    {
        public int TotalCount { get; init; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; init; }
        public List<T> Items { get; init; }
    }
}
