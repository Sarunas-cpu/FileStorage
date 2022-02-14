using Microsoft.EntityFrameworkCore;
using FileStorage.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<SFile> SFiles { get; set; }
    }
}
