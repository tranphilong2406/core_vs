using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BaiTapVideo.Areas.Admin.Models
{
    public partial class db_newsContext : DbContext
    {
        public db_newsContext()
        {
        }

        public db_newsContext(DbContextOptions<db_newsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-JFU8DHK\\SQLEXPRESS;Initial Catalog=db_news;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__account__F3DBC573A4C5BC46");

                entity.ToTable("account");

                entity.Property(e => e.Username)
                    .HasMaxLength(200)
                    .HasColumnName("username");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.Roles).HasColumnName("roles");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Slug)
                    .HasMaxLength(100)
                    .HasColumnName("slug");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Author).HasMaxLength(200);

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.FullContent).HasColumnName("full_content");

                entity.Property(e => e.Img).HasColumnName("img");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(300)
                    .HasColumnName("title");

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK__posts__Author__4AB81AF0");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK__posts__cat_id__38996AB5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
