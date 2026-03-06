using System.ComponentModel.DataAnnotations;

namespace LibApp.Models;

public class Category
{
    [Key]
    [Display(Name = "Жанр")]
    public long CategoryId { get; set; }
    
    [Required, MaxLength(100)]
    [Display(Name = "Название")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    [Display(Name = "Описание")]
    public string Description { get; set; } = string.Empty;
}