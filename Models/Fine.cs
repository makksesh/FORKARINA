using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LibApp.Models;

public class Fine
{
    [Key]
    [Display(Name = "ID Штрафа")]
    public long FineId { get; set; }

    [Display(Name = "Читатель")]
    public long ReaderId { get; set; }
    [Display(Name = "Библиотекарь")]
    public long LibrarianId { get; set; }

    [Display(Name = "Сумма штрафа")]
    public decimal Amount { get; set; }  

    [Display(Name = "Дата выставления")]
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    
    [Display(Name = "Дата оплаты")]
    public DateTime? PaidAt { get; set; }

    [Display(Name = "Причина штрафа")]
    public string? Reason { get; set; }


    [ValidateNever]
    [Display(Name = "Читатель")]
    public User Reader { get; set; } = null!;
    [ValidateNever]
    [Display(Name = "Библиотекарь")]
    public User Librarian { get; set; } = null!;
}