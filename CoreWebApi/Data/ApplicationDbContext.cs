using System;
using System.Collections.Generic;
using System.Text;
using CoreWebApi.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Post> Posts { get; set; }
    }
}
