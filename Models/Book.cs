using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LibApp.Models;

public class Book
{
    [Key]
    [Display(Name = "ID Книги")]
    public long BookId { get; set; }

    
    [Display(Name = "ID Автора")]
    public long AuthorId { get; set; }
    
    [Display(Name = "ID Жанра")]
    public long CategoryId { get; set; }

    [Required, MaxLength(255)]
    [Display(Name = "Название")]
    public string Name { get; set; } = string.Empty;
    
    [ValidateNever]    
    [Display(Name = "Автор")]
    public Author Author { get; set; } 
    [ValidateNever]
    [Display(Name = "Жанр")]
    public Category Category { get; set; } 
}