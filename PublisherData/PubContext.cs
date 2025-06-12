using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using PublisherDomain;
using PublisherDomain.Entities;

namespace PublisherData;

public class BloggingContext : DbContext
{
    public int ActivePostCountForBlog(int blogId)
         => throw new NotSupportedException();
    public DbSet<Person> People { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Animal> Animals { get; set; }
   public DbSet<Cat> Cats { get; set; }
   public DbSet<Dog> Dogs { get; set; }
   public DbSet<Student> Students { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
          "server=.;database=Blogging;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=1800000;MultipleActiveResultSets=True;integrated security=true;trust server certificate=true"
        ).LogTo(Console.WriteLine, LogLevel.Information) // 👈 Logs SQL to console
        .EnableSensitiveDataLogging(); ;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) //#E
    {

        modelBuilder.Entity<Animal>().UseTptMappingStrategy();
        //     modelBuilder.Entity<Person>()
        //.HasDiscriminator<string>("person_type")
        //.HasValue<Person>("person_base")
        //.HasValue<Student>("person_student");


        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableIdentifier = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);

            Console.WriteLine($"{entityType.DisplayName()}\t\t{tableIdentifier}");
            Console.WriteLine(" Property\tColumn");

            foreach (var property in entityType.GetProperties())
            {
                var columnName = property.GetColumnName(tableIdentifier.Value);
                Console.WriteLine($" {property.Name,-10}\t{columnName}");
            }

            Console.WriteLine();
        }



        modelBuilder.Entity<Person>()
   .HasDiscriminator(b => b.PersonType)
      .HasValue<Person>("person_base")
      .HasValue<Student>("person_student")
           .IsComplete(false);
        modelBuilder.Entity<Person>()
            .Property(e => e.PersonType)
            .HasMaxLength(200)
            .HasColumnName("person_type");

    //  modelBuilder.Entity<Student>();



        modelBuilder.Entity<Post>()
.HasMany(p => p.Comments)
.WithOne(c => c.Post);
        modelBuilder.Entity<School>().HasMany(s => s.Students).WithOne(s => s.School);

        modelBuilder.HasDbFunction(() => SqlFunctions.StandardizeUrl(default!));

        modelBuilder.HasDbFunction(typeof(BloggingContext).GetMethod(nameof(ActivePostCountForBlog), new[] { typeof(int) }))
.HasName("CommentedPostCountForBlog");

    }
}
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string PersonType { get;  set; }
}

public class Student : Person
{
    public string StudentUrl { get; set; }
    public School School { get; set; }
}

public class School
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Student> Students { get; set; }
}


public abstract class Animal
{
    public int Id { get; set; }
    public string Breed { get; set; } = null!;
}

public class Cat : Animal
{
    public string? EducationalLevel { get; set; }
}

public class Dog : Animal
{
    public string? FavoriteToy { get; set; }
}

public class CurrencyConverter : ValueConverter<Currency, decimal>
{
    public CurrencyConverter()
        : base(
            v => v.Amount,
            v => new Currency(v))
    {
    }
}

public class Currency
{
    public decimal Amount { get; }

    public Currency(decimal amount)
    {
        Amount = amount;
    }

    // Optional: override equality for value object behavior
    public override bool Equals(object obj)
    {
        return obj is Currency other && Amount == other.Amount;
    }

    public override int GetHashCode() => Amount.GetHashCode();

    public override string ToString() => $"{Amount:C}";
}
