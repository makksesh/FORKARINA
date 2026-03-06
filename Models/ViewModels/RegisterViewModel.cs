using System.ComponentModel.DataAnnotations;

namespace LibApp.Models.ViewModels;

public class RegisterViewModel
{
    [Required, MaxLength(50)]
    [Display(Name = "Логин")]
    public string Login { get; set; } = string.Empty;

    [Required, MaxLength(50), EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    [Display(Name = "ФИО")]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(20)]
    [Display(Name = "Телефон")]
    public string? PhoneNumber { get; set; }

    [Required, DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;
}