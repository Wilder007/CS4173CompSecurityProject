using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Keyless_Auth_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Keyless_Auth_Web.Data
{
    public class KeyInfoContext : DbContext
    {
        public KeyInfoContext(DbContextOptions<KeyInfoContext> options)
            : base(options)
        {
        }

        public DbSet<KeyInfo> KeyInfo { get; set; }
    }
    
    
}
