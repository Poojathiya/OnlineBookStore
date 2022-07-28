using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly LibraryDbContext _context;

        public SuppliersController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Booksuppliers.ToListAsync());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booksupplier = await _context.Booksuppliers
                .FirstOrDefaultAsync(m => m.SupId == id);
            if (booksupplier == null)
            {
                return NotFound();
            }

            return View(booksupplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupId,SupName,SupPh,SupAddress")] Booksupplier booksupplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booksupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booksupplier);
        }


        public async Task<IActionResult> Edit(int? SupId)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int SupId, [Bind("SupId,SupName,SupPh,MailId,SupAddress")] Booksupplier booksupplier)
        {
            if (SupId != booksupplier.SupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booksupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksupplierExists(booksupplier.SupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booksupplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? SupId)
        {
            if (SupId == null)
            {
                return NotFound();
            }

            var booksupplier = await _context.Booksuppliers
                .FirstOrDefaultAsync(m => m.SupId == SupId);
            if (booksupplier == null)
            {
                return NotFound();
            }

            return View(booksupplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booksupplier = await _context.Booksuppliers.FindAsync(id);
            _context.Booksuppliers.Remove(booksupplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksupplierExists(int id)
        {
            return _context.Booksuppliers.Any(e => e.SupId == id);
        }
    }
}
