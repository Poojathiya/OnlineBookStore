#nullable disable

namespace OnlineBookStore.Models
{
    public partial class NewRegistration
    {
        public NewRegistration()
        {
            Mycarts = new HashSet<Mycart>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PhNumber { get; set; }
        public string MailId { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Mycart> Mycarts { get; set; }
    }
}
