using System.ComponentModel.DataAnnotations;
using eTickets.Data.Base;

namespace eTickets.Models;

public class Cinema : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Cinema Logo")]
    [Required(ErrorMessage = "Logo is required")]
    public string LogoUrl { get; set; }

    [Display(Name = "Cinema Name")]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    [Required(ErrorMessage = "Description is required")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 50 characters")]
    public string Description { get; set; }

    public List<Movie>? Movies { get; set; }
}
