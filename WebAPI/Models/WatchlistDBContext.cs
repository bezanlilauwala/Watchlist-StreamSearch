using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebAPI3.Models
{
    public partial class WatchlistDBContext : DbContext
    {
        public WatchlistDBContext()
        {
        }

        public WatchlistDBContext(DbContextOptions<WatchlistDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Watchlist> Watchlists { get; set; }
        public virtual DbSet<WatchlistTitle> WatchlistTitles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WatchlistDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Title>(entity =>
            {
                entity.Property(e => e.ImdbRating).IsFixedLength(true);

                entity.Property(e => e.Type).IsFixedLength(true);
            });

            modelBuilder.Entity<Watchlist>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Watchlists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userid");
            });

            modelBuilder.Entity<WatchlistTitle>(entity =>
            {
                entity.HasOne(d => d.Title)
                    .WithMany(p => p.WatchlistTitles)
                    .HasForeignKey(d => d.TitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("watchlist_title_id");

                entity.HasOne(d => d.Watchlist)
                    .WithMany(p => p.WatchlistTitles)
                    .HasForeignKey(d => d.WatchlistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("title_watchlist_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
