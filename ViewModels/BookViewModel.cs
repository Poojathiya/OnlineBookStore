namespace OnlineBookStore.ViewModels
{
    public class BookViewModel : EditImageViewModel
    {
        public string BookName { get; set; }
        // public string? BookPicture { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int? SupId { get; set; }
        public int? Edition { get; set; }
        public double? Price { get; set; }
        public int? Stock { get; set; }
    }
}
