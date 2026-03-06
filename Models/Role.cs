using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LibApp.Models;


public class Role
{
    [Key]
    [Display(Name = "ID роли")]
    public long RoleId { get; set; }
    
    [ValidateNever]
    [Required, MaxLength(50)]
    [Display(Name = "Наименование роли")]
    public string Name { get; set; } 
}
