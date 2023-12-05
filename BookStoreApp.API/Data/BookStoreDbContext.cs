using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookStoreDbContext : IdentityDbContext<ApplicationUser>
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(60);
            entity.Property(e => e.LastName).HasMaxLength(60);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.Isbn).HasMaxLength(20);
            entity.Property(e => e.Summary).HasMaxLength(60);
            entity.Property(e => e.Title).HasMaxLength(30);

            entity.HasOne(d => d.Author).WithMany(p => p.Books).HasForeignKey(d => d.AuthorId);
        });
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(60);
            entity.Property(e => e.LastName).HasMaxLength(60);
        });
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "6BA21BCB-528D-47ED-A7C8-3D95F618FE06",
                Name = "User",
                NormalizedName = "USER",
            },
            new IdentityRole
            {
                Id = "C57394D0-94FF-463A-B323-FBF34E6527F9",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
            }
            );
        var hasher = new PasswordHasher<ApplicationUser>();
        modelBuilder.Entity<ApplicationUser>().HasData(
            new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = "97C482FC-EE47-44B1-B39D-B9E87036CC53",
                    FirstName = "Demo",
                    LastName = "User",
                    UserName = "Demo",
                    NormalizedUserName = "DEMO",
                    Email = "Demo@demo.com",
                    NormalizedEmail = "DEMO@DEMO.COM",
                    PasswordHash = hasher.HashPassword(null, "Pass123$"),
                },
                new ApplicationUser()
                {
                    Id = "9DCC2818-CE12-460E-9748-FB4AAA9F848D",
                    FirstName = "Demo",
                    LastName = "Admin",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@demo.com",
                    NormalizedEmail = "ADMIN@DEMO.COM",
                    PasswordHash = hasher.HashPassword(null, "Pass123$"),
                }
            }
        );

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>()
            {
                RoleId = "C57394D0-94FF-463A-B323-FBF34E6527F9",
                UserId = "9DCC2818-CE12-460E-9748-FB4AAA9F848D"
            },
            new IdentityUserRole<string>()
            {
                RoleId = "6BA21BCB-528D-47ED-A7C8-3D95F618FE06",
                UserId = "97C482FC-EE47-44B1-B39D-B9E87036CC53"
            }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
