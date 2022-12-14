using eTickets.Data.Base;
using eTickets.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTickets.Models;

public class Movie : IEntityBase
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(ErrorMessage = "Name is Required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    [Required(ErrorMessage = "Description is Required")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 255 characters")]
    public string Description { get; set; }

    [Display(Name = "Price")]
    [Required(ErrorMessage = "Price is Required")]
    [Range(0, 999.99)]
    public double Price { get; set; }

    [Url]
    [Display(Name = "ImageUrl")]
    [Required(ErrorMessage = "ImageUrl is Required")]
    public string ImageUrl { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    [Required(ErrorMessage = "Start date is Required")]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    [Required(ErrorMessage = "End date is Required")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Movie Category")]
    [Required(ErrorMessage = "Movie category is Required")]
    public MovieCategory MovieCategory { get; set; }

    public List<Actor_Movie>? Actors_Movies { get; set; }

    public int CinemaId { get; set; }
    [ForeignKey(nameof(CinemaId))] 
    public Cinema Cinema { get; set; }

    public int ProducerId { get; set; }
    [ForeignKey(nameof(ProducerId))] 
    public Producer Producer { get; set; }
}
