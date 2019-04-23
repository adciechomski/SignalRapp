using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRMVCSolution.Models
{
    public class InstrumentDataContext : DbContext
    {
        public DbSet<DummyDataCls> DummyDataCls { get; set; }

        public InstrumentDataContext(DbContextOptions<InstrumentDataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
