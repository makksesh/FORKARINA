using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LibApp.Models;

public class User
{
    [Key]
    [Display(Name = "ID Пользователя")]
    public long UserId { get; set; }
    
    [Display(Name = "ID роли")]
    public long RoleId { get; set; }

    [Required, MaxLength(50)]
    [Display(Name = "Логин")]
    public string Login { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    [Display(Name = "Пароль")]
    public string HashPass { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    [Display(Name = "Почта")]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    [Display(Name = "ФИО")]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(20)]
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }

    [ValidateNever]
    [Display(Name = "Роль")]
    public Role Role { get; set; }
    [Display(Name = "Выдачи")]
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    [Display(Name = "Штрафы")]
    public ICollection<Fine> ReaderFines { get; set; } = new List<Fine>();
    [Display(Name = "ВЫданные штрафы")]
    public ICollection<Fine> LibrarianFines { get; set; } = new List<Fine>();
}