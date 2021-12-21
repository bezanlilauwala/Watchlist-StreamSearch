using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebAPI3.Models
{
    [Table("Watchlist")]
    public partial class Watchlist
    {
        public Watchlist()
        {
            WatchlistTitles = new HashSet<WatchlistTitle>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Watchlists")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(WatchlistTitle.Watchlist))]
        public virtual ICollection<WatchlistTitle> WatchlistTitles { get; set; }
    }
}
