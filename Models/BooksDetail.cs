#nullable disable

namespace OnlineBookStore.Models
{
    public partial class BooksDetail
    {
        public BooksDetail()
        {
            Mycarts = new HashSet<Mycart>();
            //PurchaseDetails = new HashSet<PurchaseDetail>();
        }

        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int? SupId { get; set; }
        public int? Edition { get; set; }
        public double? Price { get; set; }
        public int Stock { get; set; }
        public string BookImage { get; set; }

        public virtual Booksupplier Sup { get; set; }
        public virtual ICollection<Mycart> Mycarts { get; set; }

    }
}
