using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksAppCore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(15, ErrorMessage = "MaxLength is 15")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2018, ErrorMessage = "Range is 1990 - 2018")]
        public int Year { get; set; }

        public List<Author> Authors { get; set; }
    }
}
