using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebAPI3.Models
{
    [Table("Title")]
    public partial class Title
    {
        public Title()
        {
            WatchlistTitles = new HashSet<WatchlistTitle>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Column("type")]
        [StringLength(10)]
        public string Type { get; set; }
        [Column("runtime")]
        [StringLength(50)]
        public string Runtime { get; set; }
        [Column("release_date")]
        [StringLength(50)]
        public string ReleaseDate { get; set; }
        [Column("imdb_rating")]
        [StringLength(10)]
        public string ImdbRating { get; set; }
        [Column("image")]
        public string Image { get; set; }
        [Column("plot")]
        public string Plot { get; set; }
        [Required]
        [Column("imdb_id")]
        [StringLength(50)]
        public string ImdbId { get; set; }
        [Column("runtime_str")]
        [StringLength(50)]
        public string RuntimeStr { get; set; }
        [Column("genres")]
        public string Genres { get; set; }
        [Column("tagline")]
        public string Tagline { get; set; }

        [InverseProperty(nameof(WatchlistTitle.Title))]
        public virtual ICollection<WatchlistTitle> WatchlistTitles { get; set; }
    }
}
