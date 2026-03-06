using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LibApp.Models;

public class ExampleBook
{
    [Key]
    [Display(Name = "Экземпляр книги")]
    public long ExampleBookId { get; set; }
    [Display(Name = "Издание")]
    public long VersionBookId { get; set; }

    [ValidateNever]
    [Display(Name = "Издательство")]
    public VersionBook VersionBook { get; set; } = null!;
}