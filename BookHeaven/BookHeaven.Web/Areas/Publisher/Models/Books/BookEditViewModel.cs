using BookHeaven.Common.Mapping;
using BookHeaven.Services.Models.Books;

namespace BookHeaven.Web.Areas.Publisher.Models.Books
{
    public class BookEditViewModel : BookCreateViewModel, IMapFrom<BookEditServiceModel>
    {
        public string BookPicture { get; set; }
    }
}