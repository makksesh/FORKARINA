using LibApp.Data;
using LibApp.Models;
using LibApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db) => _db = db;

    public async Task<IActionResult> BookStats(string? search)
    {
        var versions = await _db.VersionBooks
            .Include(v => v.Book).ThenInclude(b => b.Author)
            .Include(v => v.ExampleBooks)
            .ToListAsync();

        // Берём только «живые» экземпляры (не утеряны, не списаны, не в ремонте)
        var allExampleIds = versions
            .SelectMany(v => v.ExampleBooks)
            .Where(eb => eb.Status != BookStatus.Lost &&
                         eb.Status != BookStatus.Restoration &&
                         eb.Status != BookStatus.WriteOff)
            .Select(eb => eb.ExampleBookId)
            .ToList();
        
        var busyIds = await _db.Loans
            .Where(l => allExampleIds.Contains(l.ExampleBookId) &&
                        l.ReturnedAt == null)
            .Select(l => l.ExampleBookId)
            .ToHashSetAsync();
        
        var notAvailableByStatusIds = await _db.ExampleBooks
            .Where(e => allExampleIds.Contains(e.ExampleBookId) &&
                        e.Status != BookStatus.Available)
            .Select(e => e.ExampleBookId)
            .ToHashSetAsync();

        // Итоговый набор недоступных ID
        var notAvailableIds = busyIds.Union(notAvailableByStatusIds).ToHashSet();

        var query = versions
            .GroupBy(v => v.Book)
            .Select(g =>
            {
                var examples = g.SelectMany(v => v.ExampleBooks)
                    .Where(eb => eb.Status != BookStatus.Lost &&
                                 eb.Status != BookStatus.Restoration &&
                                 eb.Status != BookStatus.WriteOff)
                    .ToList();

                var total = examples.Count;
                var issued = examples.Count(eb => notAvailableIds.Contains(eb.ExampleBookId));

                return new AdminBookStatsItem
                {
                    BookId       = g.Key.BookId,
                    Title        = g.Key.Name,
                    Author       = g.Key.Author.FullName,
                    TotalCopies  = total,
                    IssuedCount  = issued
                };
            });

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.Author.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        ViewBag.Search = search;
        return View(query.OrderBy(x => x.Title).ToList());
    }
}