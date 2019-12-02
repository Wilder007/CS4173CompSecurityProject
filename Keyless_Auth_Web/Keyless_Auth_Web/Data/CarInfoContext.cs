using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Keyless_Auth_Web.Models;
using Microsoft.EntityFrameworkCore;


namespace Keyless_Auth_Web.Data
{
    public class CarInfoContext : DbContext
    {
        public CarInfoContext(DbContextOptions<CarInfoContext> options)
            : base(options)
        {
        }

        public DbSet<CarInfo> CarInfo { get; set; }
    }
}
