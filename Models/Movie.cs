using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieRentalFrontend.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Fältet får inte vara tomt")]
        [DisplayName("Film Title")]
        public string Title { get; set; }

        [Range(1999, 9999, ErrorMessage ="Värdet får inte falla utanför intervallet!")]
        [DisplayName("Year Released")]
        public int ReleaseYear { get; set; }
    }
}
