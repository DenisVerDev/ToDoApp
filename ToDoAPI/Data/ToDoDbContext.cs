using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data.Models;

namespace ToDoAPI.Data
{
    public class ToDoDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<Models.Task> Tasks { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureTasks(builder);
            ConfigureCategories(builder);
        }

        protected virtual void ConfigureTasks(ModelBuilder builder)
        {
            builder.Entity<Models.Task>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title).HasMaxLength(256).IsRequired();

                entity.Property(x => x.Description).HasMaxLength(1000); // Description can be null

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Tasks_Title", "LEN(TRIM([Title])) > 1");
                });

                entity.HasOne(x => x.Author)
                  .WithMany(x => x.Tasks)
                  .HasForeignKey(x => x.AuthorId)
                  .HasConstraintName("FK_Tasks_AuthorId")
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();
            });
        }

        protected virtual void ConfigureCategories(ModelBuilder builder)
        {
            builder.Entity<Models.Category>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name).HasMaxLength(256).IsRequired();

                entity.Property(x => x.Color).HasMaxLength(7).IsRequired();

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Categories_Name", "LEN(TRIM([Name])) > 1");
                    t.HasCheckConstraint("CK_Categories_Color", "LEN(TRIM([Color])) >= 6"); // I don't know yet if '#' is gonna be there (#RRGGBB)
                });

                entity.HasIndex(x => new { x.Name, x.AuthorId }).IsUnique();

                entity.HasOne(x => x.Author)
                  .WithMany(x => x.Categories)
                  .HasForeignKey(x => x.AuthorId)
                  .HasConstraintName("FK_Categories_AuthorId")
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();
            });
        }
    }
}
