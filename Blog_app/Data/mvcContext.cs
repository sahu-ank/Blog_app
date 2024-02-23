using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blog_app.Models;

namespace mvc.Data
{
    public class mvcContext : DbContext
    {
        public mvcContext(DbContextOptions<mvcContext> options)
            : base(options)
        {

        }

        public DbSet<Blog_app.Models.Blogs> Blogs { get; set; }

    }
}
