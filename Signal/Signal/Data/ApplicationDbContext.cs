using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Signal.Models;

namespace Signal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { set; get; }
        public DbSet<Contact> Contact { set; get; }
        public DbSet<Messages> Messages { set; get; }
        public DbSet<SignalSessions> SignalSessions { set; get; }
    }
}
