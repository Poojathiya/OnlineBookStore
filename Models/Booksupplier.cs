#nullable disable

namespace OnlineBookStore.Models
{
    public partial class Booksupplier
    {
        public Booksupplier()
        {
            BooksDetails = new HashSet<BooksDetail>();
        }

        public int SupId { get; set; }
        public string SupName { get; set; }
        public int? SupPh { get; set; }
        public string SupAddress { get; set; }
        public string MailId { get; set; }

        public virtual ICollection<BooksDetail> BooksDetails { get; set; }
    }
}
