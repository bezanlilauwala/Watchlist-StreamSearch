using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebAPI3.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Watchlists = new HashSet<Watchlist>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("user_name")]
        public string UserName { get; set; }

        [InverseProperty(nameof(Watchlist.User))]
        public virtual ICollection<Watchlist> Watchlists { get; set; }
    }
}
