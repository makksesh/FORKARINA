using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibApp.Data;
using LibApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibApp.Controllers
{
    [Authorize]
    public class ExampleBooksController : Controller
    {
        private readonly AppDbContext _context;

        public ExampleBooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ExampleBooks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ExampleBooks.Include(e => e.VersionBook);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ExampleBooks/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exampleBook = await _context.ExampleBooks
                .Include(e => e.VersionBook)
                .FirstOrDefaultAsync(m => m.ExampleBookId == id);
            if (exampleBook == null)
            {
                return NotFound();
            }

            return View(exampleBook);
        }

        // GET: ExampleBooks/Create
        public IActionResult Create()
        {
            ViewData["VersionBookId"] = new SelectList(_context.VersionBooks, "VersionBookId", "Name");
            return View();
        }

        // POST: ExampleBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExampleBookId,VersionBookId")] ExampleBook exampleBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exampleBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VersionBookId"] = new SelectList(_context.VersionBooks, "VersionBookId", "Name", exampleBook.VersionBookId);
            return View(exampleBook);
        }

        // GET: ExampleBooks/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exampleBook = await _context.ExampleBooks.FindAsync(id);
            if (exampleBook == null)
            {
                return NotFound();
            }
            ViewData["VersionBookId"] = new SelectList(_context.VersionBooks, "VersionBookId", "Name", exampleBook.VersionBookId);
            return View(exampleBook);
        }

        // POST: ExampleBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ExampleBookId,VersionBookId")] ExampleBook exampleBook)
        {
            if (id != exampleBook.ExampleBookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exampleBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExampleBookExists(exampleBook.ExampleBookId))
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
            ViewData["VersionBookId"] = new SelectList(_context.VersionBooks, "VersionBookId", "Name", exampleBook.VersionBookId);
            return View(exampleBook);
        }

        // GET: ExampleBooks/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exampleBook = await _context.ExampleBooks
                .Include(e => e.VersionBook)
                .FirstOrDefaultAsync(m => m.ExampleBookId == id);
            if (exampleBook == null)
            {
                return NotFound();
            }
            
            var hasRelations = await _context.Loans.AnyAsync(l => l.ExampleBookId == id);

            if (hasRelations)
            {
                ViewBag.ErrorMessage = "Нельзя удалить экземпляр, так как есть связанные выдачи.";
            }

            return View(exampleBook);
        }

        // POST: ExampleBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var exampleBook = await _context.ExampleBooks
                .Include(e => e.VersionBook)
                .FirstOrDefaultAsync(m => m.ExampleBookId == id);
            
            var hasRelations = await _context.Loans.AnyAsync(l => l.ExampleBookId == id);
            
            if (hasRelations || exampleBook.VersionBook != null)
            {
                ViewBag.ErrorMessage = "Нельзя удалить экземпляр, так как есть связанные выдачи или издание.";
                return View("Delete", exampleBook);
            }
            
            if (exampleBook != null)
            {
                _context.ExampleBooks.Remove(exampleBook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExampleBookExists(long id)
        {
            return _context.ExampleBooks.Any(e => e.ExampleBookId == id);
        }
    }
}
