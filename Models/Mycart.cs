#nullable disable

namespace OnlineBookStore.Models
{
    public partial class Mycart
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int? Price { get; set; }
        public int? Bill { get; set; }
        public int Qty { get; set; }

        public virtual BooksDetail Book { get; set; }
        public virtual NewRegistration User { get; set; }
    }
}
