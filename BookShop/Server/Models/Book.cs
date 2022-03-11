using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Server.Models
{
    public class Book : BaseEntity
    {
        public string Title { get; init; }

        public string Author { get; init; }

        public int PagesCount { get; init; }

        public string ShortDescription { get; init; }

        public Book() : base() { }
    }
}
