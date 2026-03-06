using System.ComponentModel.DataAnnotations;

namespace LibApp.Models;

public class Author
{
    [Key]
    [Display(Name = "ID Автора")]
    public long AuthorId { get; set; }

    [Required, MaxLength(100)]
    [Display(Name = "ФИО")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Биография")]
    public string? Bio { get; set; }
    
    public ICollection<Book> Books { get; set; } = new List<Book>();
}