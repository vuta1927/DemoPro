using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FuturisX.Security;
using FuturisX.Storage.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;

namespace DAL.Model
{
    public class DemoProContextSeed : IDbContextSeed
    {
        private ILogger<DemoProContextSeed> _logger;
        private DemoProContext _ctx;
        public DemoProContextSeed(ILogger<DemoProContextSeed> logger, DemoProContext context)
        {
            _logger = logger;
            _ctx = context;
        }

        public Type ContextType => typeof(DemoProContext);
        public async Task SeedAsync()
        {
            var policy = CreatePolicy(nameof(DemoProContext));

            await policy.ExecuteAsync(async () =>
            {
                using (_ctx)
                {
                    _ctx.Database.Migrate();

                    if (!_ctx.Users.Any(x => x.Email == "admin@demo.com"))
                    {
                        // Add 'administrator' role
                        var adminRole = await _ctx.Roles.FirstOrDefaultAsync(r => r.RoleName == "Administrator");
                        if (adminRole == null)
                        {
                            adminRole = new Role
                            {
                                RoleName = "Administrator"
                            };
                            _ctx.Roles.Add(adminRole);
                            await _ctx.SaveChangesAsync();
                        }

                        // Create admin user
                        var adminUser = _ctx.Users.FirstOrDefault(u => u.UserName == "admin");
                        if (adminUser == null)
                        {
                            adminUser = new User
                            {
                                UserName = "admin",
                                NormalizedUserName = "ADMIN",
                                Name = "admin",
                                Surname = "admin",
                                Email = "admin@demo.com",
                                NormalizedEmail = "ADMIN@DEMO.COM",
                                IsActive = true,
                                EmailConfirmed = true,
                                PasswordHash = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                            };

                            _ctx.Users.Add(adminUser);

                            _ctx.SaveChanges();

                            _ctx.UserRoles.Add(new UserRole(adminUser.Id, adminRole.Id));

                            _ctx.SaveChanges();
                        }
                    }
                }
            });
        }

        private Policy CreatePolicy(string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>()
                .WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        _logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                    }
                );
        }
    }
}
