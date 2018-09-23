using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreScaffoldexample.Model
{
    public partial class MyFeedlyDatabaseContext : DbContext
    {
        public MyFeedlyDatabaseContext()
        {
        }

        public MyFeedlyDatabaseContext(DbContextOptions<MyFeedlyDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Collections> Collections { get; set; }
        public virtual DbSet<CollectionsFeeds> CollectionsFeeds { get; set; }
        public virtual DbSet<Feeds> Feeds { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Nlogs> Nlogs { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=MyFeedlyDatabase;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collections>(entity =>
            {
                entity.HasIndex(e => e.IdUser);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Collections)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Collections_Users");
            });

            modelBuilder.Entity<CollectionsFeeds>(entity =>
            {
                entity.HasIndex(e => e.IdCollection);

                entity.HasIndex(e => e.IdFeed);

                entity.Property(e => e.IdCollection).HasColumnName("idCollection");

                entity.Property(e => e.IdFeed).HasColumnName("idFeed");

                entity.HasOne(d => d.IdCollectionNavigation)
                    .WithMany(p => p.CollectionsFeeds)
                    .HasForeignKey(d => d.IdCollection)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectionsFeeds_Collections");

                entity.HasOne(d => d.IdFeedNavigation)
                    .WithMany(p => p.CollectionsFeeds)
                    .HasForeignKey(d => d.IdFeed)
                    .HasConstraintName("FK_CollectionsFeeds_Feeds");
            });

            modelBuilder.Entity<Feeds>(entity =>
            {
                entity.Property(e => e.Hash).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasIndex(e => e.IdFeed);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.IdFeed).HasColumnName("idFeed");

                entity.HasOne(d => d.IdFeedNavigation)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.IdFeed)
                    .HasConstraintName("FK_News_Feeds");
            });

            modelBuilder.Entity<Nlogs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Level).HasMaxLength(10);

                entity.Property(e => e.Logger).HasMaxLength(128);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
