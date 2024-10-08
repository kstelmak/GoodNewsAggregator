﻿using Microsoft.EntityFrameworkCore;
using NewsAggregatorDatabase.Entities;
namespace NewsAggregatorApp.Entities
{
    public class AggregatorContext : DbContext
    {
        public DbSet<Article?> Articles { get; set; }

        //failed attempt to make an article with multiple categories
        //public DbSet<ArticleCategories?> ArticleCategories { get; set; }
        public DbSet<Category?> Categories { get; set; }
        public DbSet<Comment?> Comments { get; set; }
        public DbSet<Like?> Likes { get; set; }
		public DbSet<RefreshToken?> RefreshTokens { get; set; }
		public DbSet<Role?> Roles { get; set; }
        public DbSet<Source?> Sources { get; set; }
        public DbSet<User?> Users { get; set; }

        public AggregatorContext(DbContextOptions<AggregatorContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Source>(entity =>
            {
                entity.Property(e => e.SourceName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                //entity.Property(e => e.Title)
                //    .IsRequired()
                //    .HasMaxLength(50);

                //entity.Property(e => e.Description)
                //    .HasMaxLength(500);

                //entity.Property(e => e.Text)
                //    .IsRequired()
                //    .HasMaxLength(5000);

                entity.Property(e => e.PublicationDate)
                    .IsRequired();

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.SourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Article_Source");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Article_Category");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentText)
                    .IsRequired()
                    .HasMaxLength(500);
                
                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Article");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Article");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Like_Article");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Like_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired();

                entity.Property(e => e.PasswordHash)
                    .IsRequired();

                entity.Property(e => e.RoleId)
                    .HasDefaultValue(Guid.Parse("1FD82CBA-0198-4520-8988-C147C9AD70BE"));

                entity.Property(e => e.IsBlocked)
                    .HasDefaultValue(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            //failed attempt to make an article with multiple categories

            //modelBuilder.Entity<ArticleCategories>(entity =>
            //{  
            //    entity.HasOne(d => d.Article)
            //        .WithMany(p => p.ArticleCategories)
            //        .HasForeignKey(d => d.ArticleId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_ArticleCategories_Article");

            //    entity.HasOne(d => d.Category)
            //        .WithMany(p => p.ArticleCategories)
            //        .HasForeignKey(d => d.CategoryId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_ArticleCategories_Category");
            //});
        }
    }
}
