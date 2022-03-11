using BookShop.Server.Data;
using BookShop.Server.Models;
using BookShop.Server.SystemModels;
using BookShop.Shared.PresentationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Server.Controllers
{
    [ApiController]
    [Route( "[controller]" )]
    public class BookController : BaseController
    {
        private ApplicationDbContext _context;

        public BookController( ApplicationDbContext context, AppSettings appSettings ):base(appSettings)
        {
            _context = context;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("Create")]
        public void Create(BookModel model )
        {
            if( !ModelState.IsValid )
            {
                throw new Exception( "Ошибка в моделе" );
            }
            var book = new Book
            {
                Author = model.Author,
                PagesCount = model.PagesCount,
                ShortDescription = model.ShortDescription,
                Title = model.Title
            };
            try
            {
                _context.Books.Add( book );
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("List")]
        public PageItems<BookModel> GetList(int currentPage)
        {
            var totalPage = (int)MathF.Ceiling( _context.Books.Count() / AppSettings.PageItemCount );
            PageItems<BookModel> result = new()
            {
                TotalPages = totalPage,
                TotalCount = _context.Books.Count(),
                CurrentPage = currentPage,
                Items = _context.Books
                .Skip((currentPage - 1)* AppSettings.PageItemCount)
                .Take(AppSettings.PageItemCount)
                .Select( b => new BookModel
                {
                    Id = b.Id,
                    Author = b.Author,
                    PagesCount = b.PagesCount,
                    ShortDescription = b.ShortDescription,
                    Title = b.Title
                } )
                .ToList()
            };
            return result;
        }
    }
}
