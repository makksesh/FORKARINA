using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LibApp.Models;

public class Loan
{
    [Key]
    [Display(Name = "ID Выдачи")]
    public long LoanId { get; set; }

    [Display(Name = "ID Пользователя")]
    public long UserId { get; set; }
    
    [Display(Name = "ID экземпляра книги")]
    public long ExampleBookId { get; set; }

    [Display(Name = "Дата выдачи")]
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    
    [Display(Name = "Срок выдачи")]
    public DateTime DueDate { get; set; }

    [Display(Name = "Количество пролонгаций")]
    public int ExtensionsCount { get; set; } = 0;

    [Display(Name = "Дата возврата")]
    public DateTime? ReturnedAt { get; set; }

    [ValidateNever]
    [Display(Name = "Пользователь")]
    public User User { get; set; } = null!;
    [ValidateNever]
    [Display(Name = "Экземпляр книги")]
    public ExampleBook ExampleBook { get; set; } = null!;
}