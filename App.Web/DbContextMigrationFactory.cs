using Microsoft.EntityFrameworkCore.Design;
using App.Entry.EntityFramework;
using PoorFff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web
{
    public class DbContextMigrationFactory: IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var bootstrapper = PfBootstrapper.Create<AppWebModule>();
            bootstrapper.Initialize();
            return new AppDbContext();
        }
    }
}
