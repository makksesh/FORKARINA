using System.Security.Claims;
using LibApp.Data;
using LibApp.Models;
using LibApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Controllers;

[Authorize(Roles = "Admin,Librarian")]
public class ReturnBookController : Controller
{
    private readonly AppDbContext _context;

    public ReturnBookController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /ReturnBook/Accept/{loanId}
    [HttpGet]
    public async Task<IActionResult> Accept(long loanId)
    {
        var loan = await _context.Loans
            .Include(l => l.User)
            .Include(l => l.ExampleBook)
                .ThenInclude(eb => eb.VersionBook)
                    .ThenInclude(vb => vb.Book)
                        .ThenInclude(b => b.Author)
            .Include(l => l.ExampleBook)
                .ThenInclude(eb => eb.VersionBook)
                    .ThenInclude(vb => vb.Publisher)
            .FirstOrDefaultAsync(l => l.LoanId == loanId && l.ReturnedAt == null);

        if (loan == null)
            return NotFound();

        var now = DateTime.UtcNow;
        var isOverdue = now > loan.DueDate;
        var overdueDays = isOverdue ? (int)Math.Ceiling((now - loan.DueDate).TotalDays) : 0;

        var vm = new ReturnBookViewModel
        {
            Loan = loan,
            IsOverdue = isOverdue,
            OverdueDays = overdueDays
        };

        return View(vm);
    }

    // POST: /ReturnBook/ReturnBook
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnBook(
        long loanId,
        bool skipFine,           // нажата кнопка "без штрафа"
        decimal pricePerDay,
        bool isDamaged,
        BookCondition newCondition,
        decimal damageFineAmount,
        string? damageComment)
    {
        var loan = await _context.Loans
            .Include(l => l.User)
            .Include(l => l.ExampleBook)
            .FirstOrDefaultAsync(l => l.LoanId == loanId && l.ReturnedAt == null);

        if (loan == null) return NotFound();

        var now = DateTime.UtcNow;
        var overdueDays = (int)Math.Ceiling((now - loan.DueDate).TotalDays);
        var messages = new List<string>();

        if (!skipFine)
        {
            var librarianIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(librarianIdStr, out var librarianId))
                return Unauthorized();

            // Штраф за просрочку
            if (overdueDays > 0 && pricePerDay > 0)
            {
                _context.Fines.Add(new Fine
                {
                    ReaderId = loan.UserId,
                    LibrarianId = librarianId,
                    Amount = pricePerDay * overdueDays,
                    IssuedAt = now,
                    Reason = $"Просрочка {overdueDays} дн. × {pricePerDay:F2} ₽/день"
                });
                messages.Add($"Штраф за просрочку: {pricePerDay * overdueDays:F2} ₽");
            }

            // Штраф за повреждение
            if (isDamaged && damageFineAmount > 0)
            {
                _context.Fines.Add(new Fine
                {
                    ReaderId = loan.UserId,
                    LibrarianId = librarianId,
                    Amount = damageFineAmount,
                    IssuedAt = now,
                    Reason = $"Повреждение: {damageComment ?? "без комментария"}"
                });
                messages.Add($"Штраф за повреждение: {damageFineAmount:F2} ₽");
            }
        }

        // Обновляем состояние экземпляра
        if (loan.ExampleBook != null)
        {
            if (isDamaged && !skipFine)
                loan.ExampleBook.Condition = newCondition;

            loan.ExampleBook.Status = BookStatus.Available;
        }

        loan.ReturnedAt = now;
        await _context.SaveChangesAsync();

        TempData["Success"] = messages.Count > 0
            ? string.Join(" | ", messages)
            : "Книга принята без штрафов.";

        return RedirectToAction("Index", "Loans");
    }

}