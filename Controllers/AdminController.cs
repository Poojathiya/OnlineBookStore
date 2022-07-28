#region LIBRARIES
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;
using OnlineBookStore.ViewModels;
using System.Net;
using System.Net.Mail;
using static OnlineBookStore.Controllers.LoginController;
#endregion

namespace LibManagementSystemMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public AdminController(LibraryDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        #region ADMIN INDEX

        [NoDirectAccess]
        public IActionResult Index()
        {
            return View(_context.BooksDetails.ToList());
        }
        #endregion

        #region SEARCH BOOK

        [NoDirectAccess]
        public IActionResult SearchResult(string SearchPhrase)
        {
            return View(_context.BooksDetails.Where(i => i.BookName.Contains(SearchPhrase)).ToList());
        }
        #endregion

        #region ADD BOOKS

        [NoDirectAccess]
        public IActionResult Create()
        {
            var result = new SelectList(from i in _context.Booksuppliers
                                        select i.SupId).ToList();
            ViewBag.SupId = result;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,Author,Genre,SupId,Edition,Price,Stock,BookPicture")] BookViewModel model)
        {
            try
            {

                string uniqueFileName = ProcessUploadedFile(model);
                BooksDetail book = new()
                {
                    BookId = model.BookId,

                    BookName = model.BookName,
                    Author = model.Author,
                    Genre = model.Genre,
                    SupId = model.SupId,
                    Edition = model.Edition,
                    Price = model.Price,
                    Stock = (int)model.Stock,
                    BookImage = uniqueFileName
                };
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region BOOK DETAILS

        [NoDirectAccess]
        public async Task<IActionResult> Details(int BookId)
        {
            try
            {
                if (BookId == null)
                {
                    return NotFound();
                }
                var book = await _context.BooksDetails.FirstOrDefaultAsync(m => m.BookId == BookId);
                var bookViewModel = new BookViewModel()
                {
                    BookName = book.BookName,
                    Author = book.Author,
                    Genre = book.Genre,
                    SupId = book.SupId,
                    Edition = book.Edition,
                    Price = book.Price,
                    Stock = book.Stock,
                    ExistingImage = book.BookImage
                };
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region EDIT BOOK DETAILS

        [NoDirectAccess]
        [HttpGet]
        public async Task<IActionResult> Edit(int? BookId)
        {
            if (BookId == null)
            {
                return NotFound();
            }
            var book = await _context.BooksDetails.FindAsync(BookId);
            var bookViewModel = new BookViewModel()
            {
                BookId = book.BookId,
                BookName = book.BookName,
                Author = book.Author,
                Genre = book.Genre,
                SupId = book.SupId,
                Edition = book.Edition,
                Price = book.Price,
                Stock = book.Stock,
                ExistingImage = book.BookImage
            };
            if (book == null)
            {
                return NotFound();
            }
            return View(bookViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int BookId, BookViewModel model)
        {

            var speaker = await _context.BooksDetails.FindAsync(model.BookId);
            speaker.BookName = model.BookName;
            speaker.Author = model.Author;
            speaker.Genre = model.Genre;
            speaker.SupId = model.SupId;
            speaker.Edition = model.Edition;
            speaker.Price = model.Price;
            speaker.Stock = (int)model.Stock;
            if (model.BookPicture != null)
            {
                if (model.ExistingImage != null)
                {
                    string filePath = Path.Combine(_environment.WebRootPath, "Uploads", model.ExistingImage);
                    System.IO.File.Delete(filePath);
                }

                speaker.BookImage = ProcessUploadedFile(model);
            }
            _context.Update(speaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            return View();
        }
        #endregion

        #region DELETE BOOKS

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? BookId)
        {
            if (BookId == null)
            {
                return NotFound();
            }
            var book = await _context.BooksDetails.FirstOrDefaultAsync(m => m.BookId == BookId);
            var speakerViewModel = new BookViewModel()
            {
                BookId = book.BookId,
                BookName = book.BookName,
                Author = book.Author,
                Genre = book.Genre,
                SupId = book.SupId,
                Edition = book.Edition,
                Price = book.Price,
                Stock = book.Stock,
                ExistingImage = book.BookImage
            };
            if (book == null)
            {
                return NotFound();
            }

            return View(speakerViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int BookId)
        {
            var book = await _context.BooksDetails.FindAsync(BookId);
            string deleteFileFromFolder = Path.Combine(_environment.WebRootPath, "Uploads");
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFileFromFolder, book.BookImage);
            _context.BooksDetails.Remove(book);
            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region PROCESS UPLOAD FILE

        [NoDirectAccess]
        private string ProcessUploadedFile(BookViewModel model)
        {
            string uniqueFileName = null;
            string path = Path.Combine(_environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (model.BookPicture != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.BookPicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.BookPicture.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
        #endregion

        #region BOOK SUPPLY HISTORY

        [NoDirectAccess]
        public IActionResult BookSupplyHistory(int SupId)
        {
            List<BooksDetail> result = (from i in _context.BooksDetails.Include(x => x.Sup)
                                        where i.SupId == SupId
                                        select i).ToList();
            return View(result);
        }
        #endregion

        #region REQUEST QUOTE

        [NoDirectAccess]
        public async Task<IActionResult> RequestQuote(int? SupId)
        {
            if (SupId == null)
            {
                return NotFound();
            }
            var booksupplier = await _context.Booksuppliers.FindAsync(SupId);
            if (booksupplier == null)
            {
                return NotFound();
            }
            return View(booksupplier);
        }
        [HttpPost]
        public ActionResult RequestQuote(string SupName, string MailId, string message)
        {

            var senderEmail = new MailAddress("librarymanagement13@gmail.com", "pooja");
            var receiverEmail = new MailAddress(MailId, "Receiver");
            var password = "kigksgbmzemtqrax";
            var sub = "Hello" + SupName + "Request Quote";
            var body = message + "\\~Regards \\" + senderEmail;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }

            return RedirectToAction("Index");
        }
        #endregion

        private bool SpeakerExists(int BookId)
        {
            return _context.BooksDetails.Any(e => e.BookId == BookId);
        }
    }
}
