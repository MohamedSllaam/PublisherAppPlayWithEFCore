// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

//using (PubContext context = new PubContext())
//{
//    context.Database.EnsureCreated();
//}

internal class Program
{
    private static async Task Main(string[] args)
    {
        using var context = new BloggingContext();

        var result = context.Students.ToList();
        //var blogs = await context.Blogs
        // .OrderByDescending(blog => blog.Rating)
        // .Select(
        //     blog => new { Id = blog.BlogId, Url = SqlFunctions.StandardizeUrl(blog.Url) })
        // .ToListAsync();
        //    var blog = context.Blogs
        //.Select(
        //    b =>
        //        new { Blog = b, Post = b.Posts }).FirstOrDefault();

        //var blog = context.Blogs.Include(x=>x.Posts.Where(c=>c.Content == "Sallam").Take(2)).FirstOrDefault();

        Console.Read();



        //  var rr1 = await context.People.ToListAsync();
        ////  var rr = await context.Students.ToListAsync();
        //var query1 = from b in context.Blogs
        //             where context.ActivePostCountForBlog(b.BlogId) > 1
        //             select b;
        //// context.People.Include(person => ((Student)person).School).ToList();
        //var rr = await query1.ToListAsync();


    }

}