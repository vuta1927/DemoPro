using myCore;
using myCore.Configuration;
using myCore.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL.Model
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DemoProContext>
    {
        public DemoProContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DemoProContext>();
            //var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            builder.UseSqlServer("server=VTA\\SQLEXPRESS;Database=DemoPro;Integrated Security=true;MultipleActiveResultSets=true");
            return new DemoProContext(builder.Options, null, null);
        }
    }
}
