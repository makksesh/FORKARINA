using System.ComponentModel.DataAnnotations;

namespace LibApp.Models;

public class Publisher
{
    [Key]
    [Display(Name = "ID Издателя")]
    public long PublisherId { get; set; }
    
    [Required, MaxLength(100)]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;
    
    [Display(Name = "Описание")]
    public string? Description { get; set; }
    
    [MaxLength(100)]
    [Display(Name = "Адрес")]
    public string? Address { get; set; }
    
    [Display(Name = "Издания книг")]
    public ICollection<VersionBook> VersionBooks { get; set; } = new List<VersionBook>();
}
