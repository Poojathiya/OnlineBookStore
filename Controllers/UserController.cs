#region
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;
using Rotativa.AspNetCore;
using static OnlineBookStore.Controllers.LoginController;
#endregion

namespace OnlineBookStore.Controllers
{

    public class UserController : Controller
    {
        private readonly LibraryDbContext _context;

        public UserController(LibraryDbContext context)
        {
            _context = context;

        }
        LibraryDbContext ctx = new LibraryDbContext();

        #region USER INDEX

        [NoDirectAccess]
        public async Task<IActionResult> Index()
        {
            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            List<Mycart> result = (from i in _context.Mycarts
                                   where i.UserId == b
                                   select i).ToList();
            int count = 0;
            if (result != null)
            {

                foreach (var item in result)
                {

                    count += item.Qty;
                }
                ViewBag.count = count;
            }
            else
            {
                ViewBag.count = count;
            }

            var libraryDbContext = _context.BooksDetails.Include(b => b.Sup);
            return View(await libraryDbContext.ToListAsync());
        }
        #endregion

        #region ADD BOOKS TO CART

        [NoDirectAccess]
        public IActionResult Addtocart(int BookId)

        {
            var P = _context.BooksDetails.FirstOrDefault(i => i.BookId == BookId);
            int stcount = (int)P.Stock;
            ViewBag.Stockcount = stcount;

            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            List<Mycart> result = (from i in _context.Mycarts
                                   where i.UserId == b
                                   select i).ToList();
            double? x = 0; //check
            int count = 0;
            foreach (var item in result)
            {
                x += item.Bill;
                count += item.Qty;
            }

            string t = x.ToString();
            ViewBag.count = count;
            ViewData["total"] = x;
            return View(P);
        }
        [HttpPost]
        public IActionResult Addtocart(BooksDetail pi, int Qty, int? BookId)
        {
            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            List<Mycart> result = (from i in _context.Mycarts
                                   where i.BookId == BookId && i.UserId == b
                                   select i).ToList();
            Mycart Ac = new Mycart();

            if (result.Count == 0)
            {
                pi = _context.BooksDetails.FirstOrDefault(i => i.BookId == BookId);

                Ac.UserId = b;
                Ac.BookId = pi.BookId;
                Ac.BookName = pi.BookName;
                Ac.Author = pi.Author;
                Ac.Price = (int?)pi.Price;
                Ac.Qty = Qty;
                Ac.Bill = Ac.Price * Ac.Qty;
                _context.Add(Ac);
                _context.SaveChanges();

            }
            else
            {
                foreach (var item in result)
                {
                    Ac = _context.Mycarts.FirstOrDefault(i => i.BookId == BookId);
                    Ac.Qty = item.Qty + Qty;
                    Ac.Bill = Ac.Price * Ac.Qty;
                    _context.Update(Ac);
                    _context.SaveChanges();
                }

            }

            return RedirectToAction("Index");
        }
        #endregion


        #region VIEW CART

        [NoDirectAccess]
        public IActionResult Viewcart()
        {

            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            List<Mycart> result = (from i in _context.Mycarts
                                   where i.UserId == b
                                   select i).ToList();
            double? x = 0; //check
            int count = 0;
            foreach (var item in result)
            {
                x += item.Bill;
                count += item.Qty;
            }
            string t = x.ToString();
            ViewData["total"] = x;
            ViewBag.count = count;
            return View(result);
        }

        #endregion

        #region DELETE FROM CART

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? CartId)
        {
            if (CartId == null)
            {
                return NotFound();
            }

            var val = await _context.Mycarts
                .FirstOrDefaultAsync(m => m.CartId == CartId);
            if (val == null)
            {
                return NotFound();
            }

            return View(val);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CartId)
        {
            var val = await _context.Mycarts.FindAsync(CartId);
            _context.Mycarts.Remove(val);
            await _context.SaveChangesAsync();
            return RedirectToAction("Viewcart");
        }
        #endregion

        #region BUY
        public async Task<IActionResult> BuyDetails()
        {


            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            List<Mycart> result = (from i in _context.Mycarts
                                   where i.UserId == b
                                   select i).ToList();
            double? x = 0; //check
            int count = 0;
            foreach (var item in result)
            {
                x += item.Bill;
                count += item.Qty;
            }
            string t = x.ToString();
            ViewData["total"] = x;
            ViewBag.count = count;
            var p = _context.NewRegistrations.FirstOrDefault(i => i.UserId == b);
            ViewBag.UserName = p.UserName;
            ViewBag.MailId = p.MailId;
            ViewBag.Phnumber = p.PhNumber;

            return View(result);
        }
        #endregion

        #region CHECK OUT

        [NoDirectAccess]
        public IActionResult Emptylist()
        {

            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            List<Mycart> list = (from i in _context.Mycarts
                                 where i.UserId == b
                                 select i).ToList();

            foreach (var items in list)
            {
                var va = _context.BooksDetails.Find(items.BookId);
                va.Stock = va.Stock - items.Qty;
                _context.BooksDetails.Update(va);
                _context.SaveChanges();
            }

            foreach (var item in list)
            {
                var val = _context.Mycarts.Find(item.CartId);
                _context.Mycarts.Remove(val);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region INVOICE
        [NoDirectAccess]
        public IActionResult DemoViewAsPDF()

        {
            string UserId = HttpContext.Session.GetString("UserId");
            int b = int.Parse(UserId);
            List<Mycart> result = (from i in _context.Mycarts
                                   where i.UserId == b
                                   select i).ToList();

            return new ViewAsPdf(result);
        }
        #endregion

    }
}
