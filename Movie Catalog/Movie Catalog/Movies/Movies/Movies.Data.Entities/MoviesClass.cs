using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Movies.Data.Entities
{
    public class MoviesClass
    {       
        [Key]
        public int Id { get; set; }

        public string MovieName { get; set; }
        [Required(ErrorMessage ="Please enter Movie Genere")]
        public string MovieGenere { get; set; }
        [Required(ErrorMessage = "Please enter valid integer Number")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit Year")]
        public int MovieReleaseYear { get; set; }
        [Range(0, Int16.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Int16 MovieCollectionAmount { get; set; }

    }
}
