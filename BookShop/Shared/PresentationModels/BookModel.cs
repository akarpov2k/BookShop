using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Shared.PresentationModels
{
    public class BookModel
    {
        [DisplayName("Название")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "Название не может быть пустым")]
        public string Title { get; set; }

        [DisplayName("Автор")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Автор не может быть пустым")]
        public string Author { get; set; }

        [Range(1,double.PositiveInfinity,ErrorMessage ="Количество страниц должно быть > 0")]
        public int PagesCount { get; set; }

        public string ShortDescription { get; set; }

        public Guid Id { get; set; }
    }
}
