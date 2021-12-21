using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebAPI3.Models
{
    [Table("WatchlistTitle")]
    public partial class WatchlistTitle
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("title_id")]
        public int TitleId { get; set; }
        [Column("watchlist_id")]
        public int WatchlistId { get; set; }
        [Column("watched")]
        public bool Watched { get; set; }

        [ForeignKey(nameof(TitleId))]
        [InverseProperty("WatchlistTitles")]
        public virtual Title Title { get; set; }
        [ForeignKey(nameof(WatchlistId))]
        [InverseProperty("WatchlistTitles")]
        public virtual Watchlist Watchlist { get; set; }
    }
}
