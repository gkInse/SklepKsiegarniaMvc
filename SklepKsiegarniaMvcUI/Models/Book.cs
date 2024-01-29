using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SklepKsiegarniaMvcUI.Models
{
    public enum CoverType
    {
        Miekkie,
        Twarde,
        Ebook
    }

    [Table("Book")]
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string BookName { get; set; }

        [Required]
        [MaxLength(40)]
        public string AuthorName { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }

        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }

        [NotMapped]
        public string GenreName { get; set; }

        public CoverType CoverType { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }
    }
}
