using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using GeekShopping.IdentityServer.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(MySQLContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;

            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin))
                .GetAwaiter()
                .GetResult();

            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client))
                .GetAwaiter()
                .GetResult();

            //Admin Mock configuration
            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "peres-admin",
                Email = "peres-admin@test.com",
                EmailConfirmed = true,
                PhoneNumber = "+351 939393939",
                FirstName = "Alisson",
                LastName = "Admin"
            };

            _user.CreateAsync(admin, "Admin123-").GetAwaiter().GetResult();

            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin)
                .GetAwaiter()
                .GetResult();

            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
            }).Result;

            //Client Mock configuration
            ApplicationUser client = new ApplicationUser()
            {
                UserName = "peres-client",
                Email = "peres-client@test.com",
                EmailConfirmed = true,
                PhoneNumber = "+351 939393939",
                FirstName = "Alisson",
                LastName = "Client"
            };

            _user.CreateAsync(client, "Client123-").GetAwaiter().GetResult();

            _user.AddToRoleAsync(client, IdentityConfiguration.Client)
                .GetAwaiter()
                .GetResult();

            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client),
            }).Result;
        }
    }
}
