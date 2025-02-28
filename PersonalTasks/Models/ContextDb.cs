using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace PersonalTasks.Models;

public partial class ContextDb : DbContext
{
    public ContextDb()
    {
    }

    public ContextDb(DbContextOptions<ContextDb> options, IConfiguration configuration)
        : base(options)
    {
        try
        {
            var databaseCreate = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

            if (databaseCreate != null)
            {
                if (!databaseCreate.CanConnect()) databaseCreate.Create();
                if (!databaseCreate.HasTables()) databaseCreate.CreateTables();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public virtual DbSet<TaskItem> TaskItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__task_ite__3213E83FE481C703");

            entity.ToTable("task_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TaskItems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__task_item__user___48CFD27E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F64ABBC7B");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC572BCDF1ADC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PasswordHashed)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("password_hashed");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
