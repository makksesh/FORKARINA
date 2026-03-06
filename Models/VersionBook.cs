using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LibApp.Models;

public class VersionBook
{
    [Key]
    [Display(Name = "ID Издания")]
    public long VersionBookId { get; set; }
    
    [Display(Name = "Издатели")]
    public long PublisherId { get; set; }
    
    [Display(Name = "Книги")]
    public long BookId { get; set; }

    [MaxLength(255)]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;
    
    [Display(Name = "Дата издания")]
    public DateTime CreateAt { get; set; }
    
    [Display(Name = "Количество страниц")]
    public int CountSheets { get; set; }
    
    [ValidateNever]
    [Display(Name = "Книга")]
    public Book Book { get; set; } 
    [ValidateNever]
    [Display(Name = "Издатель")]
    public Publisher Publisher { get; set; } 
    [Display(Name = "Экземпляры книг")]
    public ICollection<ExampleBook> ExampleBooks { get; set; } = new List<ExampleBook>();

}