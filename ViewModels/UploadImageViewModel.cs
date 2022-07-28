using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.ViewModels
{
    public class UploadImageViewModel
    {
        [Display(Name = "Book Picture")]
        public IFormFile BookPicture { get; set; }
    }
}
