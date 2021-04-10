using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandboxBackend.Models;

namespace SandboxBackend.DAL
{
    public class SandboxDbContext : IdentityDbContext
    {
        public SandboxDbContext(DbContextOptions<SandboxDbContext> options)
            : base(options)
        { }
        public DbSet<JobPosting> JobPostings { get; set; }
        
    }
}
