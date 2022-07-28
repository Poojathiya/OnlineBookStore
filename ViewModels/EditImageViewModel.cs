namespace OnlineBookStore.ViewModels
{
    public class EditImageViewModel : UploadImageViewModel
    {
        public int BookId { get; set; }
        public string ExistingImage { get; set; }
    }
}
