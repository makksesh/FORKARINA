using System.ComponentModel.DataAnnotations;

namespace LibApp.Models.ViewModels;

public class LoginViewModel
{
    [Required, MaxLength(50)]
    [Display(Name = "Логин")]
    public string Login { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;
}