using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublisherDomain;
using PublisherDomain.Entities;

namespace PublisherData
{
    public class BloggingContext : DbContext
    {
        public int ActivePostCountForBlog(int blogId)
             => throw new NotSupportedException();
        public DbSet<Person> People { get; set; }
        public DbSet<School> Schools { get; set; }
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
       //     modelBuilder.Entity<Person>()
       //.HasDiscriminator<string>("person_type")
       //.HasValue<Person>("person_base")
       //.HasValue<Student>("person_student");


            modelBuilder.Entity<Person>()
       .HasDiscriminator(b => b.PersonType)
          .HasValue<Person>("person_base")
          .HasValue<Student>("person_student");

            modelBuilder.Entity<Person>()
                .Property(e => e.PersonType)
                .HasMaxLength(200)
                .HasColumnName("person_type");

          modelBuilder.Entity<Student>();



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
}